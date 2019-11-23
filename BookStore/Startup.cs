using System;
using AutoMapper;
using BookStore.Models;
using BookStore.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BookStore
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
            services.AddAuthentication()
                    .AddCookie("Customer", o =>
                    {
                        o.LoginPath = "/User/Login";
                        o.AccessDeniedPath = "/User/Access";
                        o.LogoutPath = "/User/Logout";
                    })
                    .AddCookie("Admin", o =>
                    {
                        o.LoginPath = "/Admin/Login/Index";
                        o.AccessDeniedPath = "/Admin/Login/Access";
                        o.LogoutPath = "/Admin/Login/Logout";
                    });
            services.AddDbContext<MyDBContext>(option => option.UseSqlServer(Configuration.GetConnectionString("MyDb")));
            services.AddAutoMapper(typeof(Startup));
            services.AddSession(p =>
            {
                p.IdleTimeout = TimeSpan.FromMinutes(30);
                p.Cookie.IsEssential = true;
            });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            // 2FA
            services.AddHttpClient();
            services.AddTransient<IAuthy, Authy>();
            //SMS
            services.AddTransient<ISmsService, SmsService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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
            app.UseAuthentication();
            app.UseSession();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                  name: "Admin",
                  template: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );
            });

        }
    }
}
