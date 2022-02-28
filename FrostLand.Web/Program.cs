using FrostLand.Core;
using FrostLand.Runtime;
using FrostLand.Web.Authentication;
using FrostLand.Web.Controllers;
using FrostLand.Web;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Configuration;
using NonSucking.Framework.Extension.IoC;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

var typeContainer = TypeContainer.Get<ITypeContainer>();

CallStartUp<RuntimeStartup>(typeContainer);
CallStartUp<ControllerBuilder>(typeContainer);

var userSessionService = typeContainer.Get<IUserSessionService>();

var tokenProvider = new TokenProvider($"{nameof(FrostLand)}.{nameof(FrostLand.Web)}", userSessionService);
tokenProvider.LoadOrCreateKey();

typeContainer.Register<ISessionTokenProvider>(tokenProvider);
typeContainer.Register(tokenProvider);
typeContainer.Register<IControllerActivator, CustomControllerActivator>(InstanceBehaviour.Singleton);
typeContainer.Register<IConfiguration>(builder.Configuration);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// In production, the Angular files will be served from this directory
builder.Services.AddSpaStaticFiles(configuration =>
{
    configuration.RootPath = "ClientApp";
});

builder
    .Services
    .AddAuthentication(auth =>
    {
        auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(jwt =>
    {
        jwt.SaveToken = true;
        jwt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            ValidateAudience = false,
            IssuerSigningKey = tokenProvider.Key,
            ValidIssuer = tokenProvider.Issuer
        };
        jwt.SecurityTokenValidators.Clear();
        jwt.SecurityTokenValidators.Add(tokenProvider);

#if DEBUG
        jwt.RequireHttpsMetadata = false;
#endif
    });

var controllerActivator = typeContainer.Get<IControllerActivator>();
builder.Services.AddSingleton(controllerActivator);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseSpaStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

//HACK: Only for Closed BETA builds
//app.Use((context, next) =>
//{
//    if (context.Request.Headers.TryGetValue("closed-beta", out var betaKey))
//    {
//        return next();
//    }
//    else
//    {
//        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
//        return Task.CompletedTask;
//    }

//});

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller}/{action=Index}/{id?}");

app.MapControllers();


app.UseSpa(spa =>
{
    //    // To learn more about options for serving an Angular SPA from ASP.NET Core,
    //    // see https://go.microsoft.com/fwlink/?linkid=864501

    spa.Options.SourcePath = "ClientApp";
    if (app.Environment.IsDevelopment())
    {
    }

});

//When using wwwroot
//app.MapFallbackToFile("index.html");

app.Run();

static void CallStartUp<T>(ITypeContainer typeContainer) where T : IStartUp
{
    T.Register(typeContainer);
}