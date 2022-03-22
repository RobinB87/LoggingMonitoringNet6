using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

//// Configure http logging
//builder.Services.AddHttpLogging(logging =>
//{
//    // https://bit.ly/aspnetcore6-httplogging
//    logging.LoggingFields = HttpLoggingFields.All;
//    logging.MediaTypeOptions.AddText("application/javascript");
//    logging.RequestBodyLogLimit = 4096;
//    logging.ResponseBodyLogLimit = 4096;
//});

//// Configure W3C logging
//var path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
//builder.Services.AddW3CLogging(o =>
//{
//    // https://bit.ly/aspnetcore6-w3clogger
//    o.LoggingFields = W3CLoggingFields.All;
//    o.FileSizeLimit = 5 * 1024 * 1024;
//    o.RetainedFileCountLimit = 2;
//    o.FileName = "CarvedRock-W3C-UI";
//    o.LogDirectory = Path.Combine(path, "logs");
//    o.FlushInterval = TimeSpan.FromSeconds(2);
//});

JwtSecurityTokenHandler.DefaultMapInboundClaims = false;
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "Cookies";
    options.DefaultChallengeScheme = "oidc";
})
.AddCookie("Cookies")
.AddOpenIdConnect("oidc", options =>
{
    options.Authority = "https://demo.duendesoftware.com";
    options.ClientId = "interactive.confidential";
    options.ClientSecret = "secret";
    options.ResponseType = "code";
    options.Scope.Add("openid");
    options.Scope.Add("profile");
    options.Scope.Add("email");
    options.Scope.Add("api");
    options.Scope.Add("offline_access");
    options.GetClaimsFromUserInfoEndpoint = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        NameClaimType = "email"
    };
    options.SaveTokens = true;
});
builder.Services.AddHttpContextAccessor();
builder.Services.AddRazorPages();
builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler("/Error");
// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
app.UseHsts();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages().RequireAuthorization();

app.Run();
