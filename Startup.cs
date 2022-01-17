using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Formatters;
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
using Rotativa.AspNetCore;
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

            services.AddTransient<IRepository<AkBank, int, string>, AkBankRepository>();
            services.AddTransient<IRepository<JKW, int, string>, JKWRepository>();
            services.AddTransient<IRepository<JBank, int, string>, JBankRepository>();
            services.AddTransient<IRepository<JNegeri, int, string>, JNegeriRepository>();
            services.AddTransient<IRepository<AkCarta, int, string>, AkCartaRepository>();
            services.AddTransient<IRepository<AkAkaun, int, string>, AkAkaunRepository>();
            services.AddTransient<IRepository<AkTerima, int, string>, AkTerimaRepository>();
            services.AddTransient<ListViewIRepository<AkTerima1, int>, AkTerima1Repository>();
            services.AddTransient<ListViewIRepository<AkTerima2, int>, AkTerima2Repository>();
            services.AddTransient<IRepository<AkPO, int, string>, AkPORepository>();
            services.AddTransient<ListViewIRepository<AkPO1, int>, AkPO1Repository>();
            services.AddTransient<ListViewIRepository<AkPO2, int>, AkPO2Repository>();
            services.AddTransient<IRepository<AkPembekal, int, string>, AkPembekalRepository>();
            services.AddTransient<IRepository<AkJurnal, int, string>, AkJurnalRepository>();
            services.AddTransient<ListViewIRepository<AkJurnal1, int>, AkJurnal1Repository>();
            services.AddTransient<IRepository<AkBelian, int, string>, AkBelianRepository>();
            services.AddTransient<ListViewIRepository<AkBelian1, int>, AkBelian1Repository>();
            services.AddTransient<ListViewIRepository<AkBelian2, int>, AkBelian2Repository>();
            services.AddTransient<AppLogIRepository<AppLog, int>, AppLogRepository>();
            services.AddTransient<IRepository<AkPV, int, string>, AkPVRepository>();
            services.AddTransient<ListViewIRepository<AkPV1, int>, AkPV1Repository>();
            services.AddTransient<ListViewIRepository<AkPV2, int>, AkPV2Repository>();
            services.AddTransient<IRepository<SuPekerja, int, string>, SuPekerjaRepository>();
            services.AddTransient<ListViewIRepository<SuTanggunganPekerja, int>, SuTanggunganPekerjaRepository>();
            services.AddTransient<IRepository<JAgama, int, string>, JAgamaRepository>();
            services.AddTransient<IRepository<JBangsa, int, string>, JBangsaRepository>();
            services.AddTransient<IRepository<JJawatanPekerja, int, string>, JJawatanPekerjaRepository>();
            services.AddTransient<IRepository<JCaraBayar, int, string>, JCaraBayarRepository>();
            services.AddTransient<IRepository<AbBukuVot, int, string>, AbBukuVotRepository>();
            services.AddTransient<IRepository<JJantina, int, string>, JJantinaRepository>();
            services.AddScoped(ss => SessionCartTerima.GetCart(ss));
            services.AddScoped(ss => SessionCartPO.GetCart(ss));
            services.AddScoped(ss => SessionCartJurnal.GetCart(ss));
            services.AddScoped(ss => SessionCartBelian.GetCart(ss));
            services.AddScoped(ss => SessionCartPV.GetCart(ss));
            services.AddScoped(ss => SessionCartPekerja.GetCart(ss));

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddControllersWithViews()
                .AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );

            services.AddAuthorization(options=>
            {
                options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
            });

            services.AddMvc(f =>
            {
                f.OutputFormatters.RemoveType
                (typeof(HttpNoContentOutputFormatter));
                f.OutputFormatters.Insert(0, new
                HttpNoContentOutputFormatter
                {
                    TreatNullValueAsNoContent = false
                });
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, UserManager<IdentityUser> userManager)
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

            SeedData.SeedUsers(userManager);
            RotativaConfiguration.Setup(env.ContentRootPath, "wwwroot/plugins/Rotativa");

        }
    }
}
