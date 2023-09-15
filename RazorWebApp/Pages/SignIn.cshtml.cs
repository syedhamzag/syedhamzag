using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RazorWebApp.Helpers;
using RazorWebApp.ViewModels;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Linq;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Collections.Generic;

namespace RazorWebApp.Pages;
[ValidateAntiForgeryToken]
public class SignInModel : PageModel
{
    private readonly ILogger<SignInModel> _logger;
    SecurityHelper _SecurityHelper = new SecurityHelper();

    public SignInModel(ILogger<SignInModel> logger)
    {
        _logger = logger;
    }
    public IActionResult OnGet()
    {
        var _Key = HttpContext.Session.GetString("strkey");
        if (!string.IsNullOrEmpty(_Key))
        {
            return Redirect("/");
        }
        return Page();
    }


    public async Task<IActionResult> OnPostLoginAsync([FromBody] SignInViewModel _SignInViewModel)
    {
        ApiResponse apiResponse = await _SecurityHelper.LoginAsync(_SignInViewModel);
        if (apiResponse.statusCode == StatusCodes.Status200OK.ToString())
        {
            HttpContext.Session.SetString("strkey", apiResponse.message.ToString().Trim());
            if(_SignInViewModel.RememberMe)
            {
                var ExpiresUtc = DateTime.UtcNow.AddMonths(1);
                var userClaims = new List<Claim>()
                {
                    new Claim("Token", apiResponse.message.ToString().Trim()),
                    new Claim("Email", _SignInViewModel.Email.ToString()),
                    new Claim("ExpiresUtc", ExpiresUtc.ToString())
                };

                var grantMyIdentity = new ClaimsIdentity(userClaims, "User Identity");
                var userPrincipal = new ClaimsPrincipal(new[] { grantMyIdentity });
                await HttpContext.SignInAsync(userPrincipal, new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = ExpiresUtc                         
                });
            }
            await UserLoginInfoAsync();
        }
        return new JsonResult(apiResponse);

    }

    public async Task<ApiResponse> LoginExternalAsync(SignInExternalViewModel _SignInViewModel)
    {
        ApiResponse apiResponse = new ApiResponse();
        apiResponse = await _SecurityHelper.LoginExternalAsync(_SignInViewModel);
        return apiResponse;
    }

    public async Task<IActionResult> OnGetGenerateOTPAsync([FromHeader] string UserId)
    {

        ApiResponse apiResponse = await _SecurityHelper.GenerateOTPAsync(UserId);
        return new JsonResult(apiResponse);
    }
    public async Task<IActionResult> OnGetConfirmOTPAsync([FromHeader] string OTP)
    {
        ApiResponse apiResponse = await _SecurityHelper.ConfirmOTPAsync(OTP);

        return new JsonResult(apiResponse);
    }

    public async Task<IActionResult> OnGetGoogleLogin()
    {
        var _Key = HttpContext.Session.GetString("strkey");
        if (!string.IsNullOrEmpty(_Key))
        {
            return Page();//  RedirectToPage("/Index");
        }

        await HttpContext.SignOutAsync();
        HttpContext.Session.SetString("strkey", "");

        var accessToken = await HttpContext.GetTokenAsync(
          GoogleDefaults.AuthenticationScheme, "access_token");

        var properties = new AuthenticationProperties { };
        properties.RedirectUri = "/SignIn?handler=GoogleLoginCallback";
        return Challenge(properties, GoogleDefaults.AuthenticationScheme);
    }
    public async Task OnGetGoogleLogout()
    {
        await HttpContext.SignOutAsync();
        HttpContext.Session.SetString("strkey", "");
    }
    public async Task<IActionResult> OnGetGoogleLoginCallback()
    {
        var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        var claims = result.Principal.Identities.FirstOrDefault()
            .Claims.Select(claim => new
            {
                claim.Issuer,
                claim.OriginalIssuer,
                claim.Type,
                claim.Value
            });
        String _Email = string.Empty, _ExternalId = string.Empty, _FirstName_EN = string.Empty, _LastName_EN = string.Empty, _PhotoUrl = string.Empty;
        foreach (var item in claims)
        {
            //Console.WriteLine(item);
            var _Type = item.Type.Split("/");
            switch (_Type.LastOrDefault())
            {
                case "emailaddress":
                    _Email = item.Value;
                    break;
                case "nameidentifier":
                    _ExternalId = item.Value;
                    break;
                case "givenname":
                    _FirstName_EN = item.Value;
                    break;
                case "surname":
                    _LastName_EN = item.Value;
                    break;
                default:
                    break;

            }
            if (item.Type == "image") { _PhotoUrl = item.Value; }
            //  Console.WriteLine("{0} {1}", item.Type, item.Value);

        }

        SignInExternalViewModel _SignInExternalViewModel = new SignInExternalViewModel
        {
            Email = _Email,
            ExternalId = _ExternalId,
            FirstName_EN = _FirstName_EN,
            LastName_EN = _LastName_EN,
            PhoneNumber = "",
            PhotoUrl = _PhotoUrl,

        };
        var _response = await LoginExternalAsync(_SignInExternalViewModel);
        if (_response.statusCode == StatusCodes.Status200OK.ToString())
        {
            HttpContext.Session.SetString("strkey", _response.message.ToString().Trim());
            var _LoginUserViewModel = (LoginUserViewModel)_response.data;                                                         //            UserLoginInfo(_LoginUserViewModel);
            await UserLoginInfoAsync();
        }
        return RedirectToPage("Index");
    }
    public async Task UserLoginInfoAsync()
    {
        ApiResponse apiResponse = new();
        var _Key = HttpContext.Session.GetString("strkey");
        apiResponse = await _SecurityHelper.GetUserLoginInfoAsync(_Key);
        string Name_EN = "", Name_AR = "", Email = "", PhoneNumber = "", PhotoUrl = "";

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

