using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ArmadilloParty.Support;
using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.HttpOverrides;

namespace ArmadilloParty
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureSameSiteNoneCookies();

            services
                .AddAuth0WebAppAuthentication(options =>
                {
                    options.Domain = Configuration["Auth0:Domain"];
                    options.ClientId = Configuration["Auth0:ClientId"];
                });

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = Configuration["Auth0:Domain"];
                    options.Audience = Configuration["Auth0:Audience"];
                    // If the access token does not have a `sub` claim, `User.Identity.Name` will be `null`. Map it to a different claim by setting the NameClaimType below.
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        NameClaimType = ClaimTypes.NameIdentifier
                    };
                });


            services.AddAuthorization(options =>
            {
                options.AddPolicy("puzzlesolver", policy => policy.Requirements.Add(new HasScopeRequirement("puzzlesolver", Configuration["Auth0:Audience"])));
            });

            services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();

            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            //forward headers from the LB
            var forwardOpts = new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedProto | ForwardedHeaders.XForwardedFor
            };
            //TODO: Set this up to only accept the forwarded headers from the load balancer
            forwardOpts.KnownNetworks.Clear();
            forwardOpts.KnownProxies.Clear();
            app.UseForwardedHeaders(forwardOpts);

   
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
