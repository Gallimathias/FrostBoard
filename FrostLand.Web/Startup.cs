using FrostLand.Core;
using FrostLand.Web.Authentication;
using FrostLand.Web.Controllers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using NonSucking.Framework.Extension.IoC;

namespace FrostLand.Web
{
    public class Startup
    {
        private readonly TokenProvider tokenProvider;
        private readonly ITypeContainer typeContainer;

        public Startup(IConfiguration configuration)
        {
            typeContainer = TypeContainer.Get<ITypeContainer>();
            Configuration = configuration;

            Runtime.Startup.Register(typeContainer);

            var userSessionService = typeContainer.Get<IUserSessionService>();
            tokenProvider = new TokenProvider($"{nameof(FrostLand)}.{nameof(Web)}", userSessionService);
            tokenProvider.LoadOrCreateKey();

            typeContainer.Register<ISessionTokenProvider>(tokenProvider);
            typeContainer.Register(tokenProvider);            
            typeContainer.Register<IControllerActivator, CustomControllerActivator>(InstanceBehaviour.Singleton);

            ControllerBuilder.Register(typeContainer);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp";
            });

            services
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
            services.AddSingleton(controllerActivator);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                //    // To learn more about options for serving an Angular SPA from ASP.NET Core,
                //    // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";
                if (env.IsDevelopment())
                {
                }

            });
        }
    }
}
