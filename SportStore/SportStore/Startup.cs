using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SportStore.Models;

namespace SportStore
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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration["Data:SportsStore:ConnectionString"]));
            services.AddTransient<IProductRepository, EFProductRepository>();
            services.AddScoped<Cart>(sc => SessionCart.GetCart(sc));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            
            services.AddMemoryCache();
            services.AddSession(options => {
                options.IdleTimeout = TimeSpan.FromMinutes(1);
            });
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseStatusCodePages();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession(options: new SessionOptions(){IdleTimeout = new TimeSpan(0, 20, 0)});
            
            app.UseRouting();
            app.UseAuthorization();

            SetupEndpoints(app);
            
            SeedData.EnsurePopulated(app);
        }

        private void SetupEndpoints(IApplicationBuilder app)
        {
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: null,
                    pattern: "{category}/Page{productPage:int}",
                    defaults: new {Controller = "Product", action = "List"});
                
                endpoints.MapControllerRoute(
                    name: null,
                    pattern: "Page{productPage}",
                    defaults: new {Controller = "Product", action = "List", productPage = 1});
                
                endpoints.MapControllerRoute(
                    name: null,
                    pattern: "{category}",
                    defaults: new {Controller = "Product", action = "List", productPage = 1});

                endpoints.MapControllerRoute(
                    name: "pagination",
                    pattern: "",
                    defaults: new {Controller = "Product", action = "List", productPage = 1});
                
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Product}/{action=List}/{id?}");
            });
        }
    }
}