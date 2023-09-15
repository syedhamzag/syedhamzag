using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RazorWebApp.Helpers;
using RazorWebApp.ViewModels;

namespace RazorWebApp.Pages;

public class IndexModel : PageModel
{
    ConfigurationHelper _ConfigurationHelper = new ConfigurationHelper();
    string _BaseUri = ""; string _Url = "";
    string filePath;
    private readonly ILogger<IndexModel> _logger;
    string FolderPath = string.Empty;
    string ImageUrlPath = string.Empty;
    string Language = "";
    public string LanguageBtn { get; set; }
    public string Name_EN {get;set;}
    public string Name_AR  {get;set;}
    public string Email {get;set;} 
    public string PhoneNumber {get;set;}
    public string PhotoUrl {get;set;}
    SecurityHelper _SecurityHelper = new SecurityHelper();
    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
        _BaseUri = _ConfigurationHelper.GetGWAPI() + "/api/Translation/v1/";
        ImageJsonViewModel _ImageJsonViewModel = _ConfigurationHelper.GetImagesUrl();
        FolderPath = _ImageJsonViewModel.ImageLocation;
        ImageUrlPath = _ImageJsonViewModel.ImageLogoUrl;
    }

    public async Task OnGet()
    {
        var currentTime = DateTime.UtcNow;
        var cookie = Request.Cookies["UserLoginCookie"]; 
        ClaimsIdentity User = HttpContext.User.Identity as ClaimsIdentity;  
        if(!string.IsNullOrEmpty(User.AuthenticationType))
        {
            var Token = User?.Claims.FirstOrDefault(s => s.Type == "Token").Value.ToString();     
            var Email = User?.Claims.FirstOrDefault(s => s.Type == "Email").Value.ToString(); 
            var ExpiresUtc = User?.Claims.FirstOrDefault(s => s.Type == "ExpiresUtc").Value.ToString(); 
            if(currentTime <= DateTime.Parse(ExpiresUtc))
            {
                if(!string.IsNullOrEmpty(Token) && !string.IsNullOrEmpty(Email))
                {
                    HttpContext.Session.SetString("strkey", Token);
                    await UserLoginInfoAsync();
                }

            }
        }

    }
    public ActionResult OnGetChangeLanguageAsync(string Language)
    {
        string Direction = Language == "AR" ? "rtl" : "ltr";
        HttpContext.Session.SetString("Language", Language);
        HttpContext.Session.SetString("Direction", Direction);
        return new JsonResult(StatusCodes.Status200OK);


    }

    public async Task<IActionResult> OnGetLogoutAsync()
    {
        HttpContext.Session.SetString("strkey", "");
        ViewData["profileView"] = "display:none !important";
        ViewData["signView"] = "";
        await HttpContext.SignOutAsync();
        return RedirectToPage("Index");
    }

    public ActionResult OnGetHome()
    {
        Language = HttpContext.Session.GetString("Language") == null ? Language : HttpContext.Session.GetString("Language").ToString();

        ApiResponse apiResponse = new();
        filePath = FolderPath + "\\CMS\\JSON\\home\\" + "StaticPagesSection." + Language + ".json";
        if (System.IO.File.Exists(filePath))
        {
            string jsonData = System.IO.File.ReadAllText(filePath);
            JObject jsonObject = JObject.Parse(jsonData);


            string Content = jsonObject["Content"].ToString();


            apiResponse.statusCode = StatusCodes.Status200OK.ToString();
            apiResponse.data = Content;
            return new JsonResult(apiResponse);
        }
        apiResponse.statusCode = StatusCodes.Status404NotFound.ToString();
        apiResponse.message = "Record not found";
        return new JsonResult(apiResponse);
    }
    public async Task UserLoginInfoAsync()
    {
        ApiResponse apiResponse = new();
        var _Key = HttpContext.Session.GetString("strkey");
        apiResponse = await _SecurityHelper.GetUserLoginInfoAsync(_Key);

        if (apiResponse.statusCode == StatusCodes.Status200OK.ToString())
        {
            LoginUserViewModel _LoginUserViewModel = (LoginUserViewModel)apiResponse.data;
            Name_EN = _LoginUserViewModel.FirstName_EN + " " + _LoginUserViewModel.LastName_EN;
            Name_AR = _LoginUserViewModel.FirstName_AR + " " + _LoginUserViewModel.LastName_AR;
            Email = _LoginUserViewModel.Email;
            PhoneNumber = string.IsNullOrEmpty(_LoginUserViewModel.PhoneNumber) ? "" : _LoginUserViewModel.PhoneNumber;
            PhotoUrl = string.IsNullOrEmpty(_LoginUserViewModel.PhotoUrl) ? "../../images/user.png" : _LoginUserViewModel.PhotoUrl;
        }

        HttpContext.Session.SetString("Name_EN", Name_EN);
        HttpContext.Session.SetString("Name_AR", Name_AR);
        HttpContext.Session.SetString("Email", Email);
        HttpContext.Session.SetString("PhoneNumber", PhoneNumber);
        HttpContext.Session.SetString("PhotoUrl", PhotoUrl);
    }
}
