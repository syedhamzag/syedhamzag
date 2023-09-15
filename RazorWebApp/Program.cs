using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.CookiePolicy;
using RazorWebApp.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSession(option => option.IdleTimeout = TimeSpan.FromDays(100));
builder.Services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");
builder.Services.AddScoped<IApiRepository, ApiRepository>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddRazorPages();

builder.Services
   .AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
    })
    .AddCookie(options => { options.LoginPath = "/signin-google"; })
    .AddGoogle(options =>
    {
        options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
        options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
        //options.ClaimActions.MapJsonKey("urn:google:picture", "picture", "url");
        //options.ClaimActions.MapJsonKey(ClaimTypes.OtherPhone, "otherphone");

        options.ClaimActions.MapJsonKey("image", "picture");
        options.ClaimActions.MapJsonKey("otherphone", "otherphone");
        options.ClaimActions.MapJsonKey("phoneNumbers", "phoneNumbers");
        options.ClaimActions.MapJsonKey("genders", "genders");
        options.ClaimActions.MapJsonKey("gender", "gender");
        options.ClaimActions.MapJsonKey("locale", "locale");
        options.Scope.Add("https://www.googleapis.com/auth/user.phonenumbers.read");
        options.SaveTokens = true;

    });
    TimeSpan expiration_cookie_and_session = TimeSpan.FromHours(2);
     builder.Services.AddAuthentication("CookieAuthentication")
             .AddCookie("CookieAuthentication", config =>
              {
                  config.Cookie.Name = "UserLoginCookie";
                  config.LoginPath = "/SignIn";
                  config.SlidingExpiration = true;
                  config.ExpireTimeSpan = expiration_cookie_and_session;
                  config.EventsType = typeof(MyCookieAuthenticationEvents);
              });
     builder.Services.AddScoped<MyCookieAuthenticationEvents>();
     builder.Services.AddSession(options => {
              options.IdleTimeout = expiration_cookie_and_session;
         });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseSession();
app.UseCookiePolicy(new CookiePolicyOptions
{
    HttpOnly = HttpOnlyPolicy.Always,
    Secure = CookieSecurePolicy.Always
});
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
