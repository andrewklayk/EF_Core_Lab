using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CoreTest1.Models;
using Microsoft.EntityFrameworkCore;
using CoreTest1.Data;
using System.Globalization;
using Microsoft.AspNetCore.Localization;

namespace CoreTest1
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            /*Server=.; Database=db; Persist Security Info=True; Integrated Security=SSPI;
                Server=.; Database=dddd; Persist Security Info=True; User Id=uuuu; password=pppp;*/

            var conn = Configuration.GetConnectionString("DefaultConnection");
            
            //REPLACE LAB 4 WITH TRUE FILE
            //var conn = "Server=.\\SQLEXPRESS; Database=Lab_6_db; Persist Security Info=True; User Id = admin; password = admin;";

            services.AddDbContext<RocketContext>(options => options.UseSqlServer(conn));
            
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            var ci = new CultureInfo("en-US");
            ci.NumberFormat.CurrencySymbol = "€";

            CultureInfo.DefaultThreadCurrentCulture = ci;
            CultureInfo.DefaultThreadCurrentUICulture = ci;
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(ci),
                SupportedCultures = new List<CultureInfo>
    {
        ci,
                },
                SupportedUICultures = new List<CultureInfo>
    {
        ci,
                }
            });
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
