using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options => { options.LoginPath = "/User/Login"; options.ExpireTimeSpan = TimeSpan.FromMinutes(20); });//


builder.Services.AddLocalization(opt => { opt.ResourcesPath = "Resources"; });//

builder.Services.AddMvc()
    .AddViewLocalization
    (Microsoft.AspNetCore.Mvc.Razor.LanguageViewLocationExpanderFormat.Suffix)
    .AddDataAnnotationsLocalization();//

builder.Services.Configure<RequestLocalizationOptions>(
    options =>
    {
        var supportedCultures = new List<CultureInfo>
        {
            new CultureInfo("en"),
            new CultureInfo("bg"){ NumberFormat = new NumberFormatInfo(){NumberDecimalSeparator = "." } },
			new CultureInfo("ru"){ NumberFormat = new NumberFormatInfo(){NumberDecimalSeparator = "." } }
            //mnogo vajno da sloja number format na . inache pri izprashtane na drobni chisla shte maha zapetaikata
    
        };
        options.DefaultRequestCulture = new RequestCulture("bg");
        options.SupportedCultures = supportedCultures;
        options.SupportedUICultures = supportedCultures;
    });//

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();//

app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);//

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
