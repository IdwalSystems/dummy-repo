using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MSNK.Data;
using MSNK.Models.Administration;
using MSNK.Models.Modules;
using MSNK.Models.Modules.Cart;
using MSNK.Models.Modules.Cart.Session;
using MSNK.Models.Modules.EFRepository;
using MSNK.Models.Modules.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK
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
            services.AddSession();
            services.AddMemoryCache();
            
            services.AddDbContext<ApplicationDbContext>(options=>options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddIdentity<IdentityUser,IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();
            services.Configure<IdentityOptions>(opt =>
                {
                    opt.Password.RequiredLength = 5;
                    opt.Password.RequireLowercase = true;
                    opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(20);
                    opt.Lockout.MaxFailedAccessAttempts = 3;
                }
            );
            services.ConfigureApplicationCookie(opt =>
            {
                opt.AccessDeniedPath = new PathString("/Home/Accessdenied");
            });

            services.AddTransient<IRepository<AkBank, int>, AkBankRepository>();
            services.AddTransient<IRepository<JKW, int>, JKWRepository>();
            services.AddTransient<IRepository<JBank, int>, JBankRepository>();
            services.AddTransient<IRepository<JNegeri, int>, JNegeriRepository>();
            services.AddTransient<IRepository<AkCarta, int>, AkCartaRepository>();
            services.AddTransient<IRepository<AkAkaun, int>, AkAkaunRepository>();
            services.AddTransient<IRepository<AkTerima, int>, AkTerimaRepository>();
            services.AddTransient<IRepository<AkTerima1, int>, AkTerima1Repository>();
            services.AddTransient<IRepository<AkTerima2, int>, AkTerima2Repository>();
            //test
            services.AddScoped(ss => SessionCartTerima.GetCart(ss));

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

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
            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
