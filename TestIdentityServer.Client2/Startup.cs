using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace TestIdentityServer.Client2
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(opt =>
            {
                opt.DefaultScheme = "Cookies";
                opt.DefaultChallengeScheme = "oidc";
            }).AddCookie("Cookies", opt => {

                opt.AccessDeniedPath = "/Home/AccessDenied";

            }).AddOpenIdConnect("oidc", config =>
            {
                config.SignInScheme = "Cookies";
                config.Authority = "https://localhost:5001";
                config.ClientId = "Client2-Mvc";
                config.ClientSecret = "secret";
                config.ResponseType = "code id_token";
                config.GetClaimsFromUserInfoEndpoint = true;
                config.SaveTokens = true;
                config.Scope.Add("api1.read");
                config.Scope.Add("offline_access");
                config.Scope.Add("CountryAndCity");
                config.Scope.Add("Roles");
                config.ClaimActions.MapUniqueJsonKey("country", "country");
                config.ClaimActions.MapUniqueJsonKey("city", "city");
                config.ClaimActions.MapUniqueJsonKey("role", "role");

                config.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {
                    RoleClaimType = "role"
                };

            });
            services.AddControllersWithViews();
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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
