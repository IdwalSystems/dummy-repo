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

            services.AddTransient<IRepository<AkBank, int>, AkBankRepository>();
            services.AddTransient<IRepository<JKW, int>, JKWRepository>();
            services.AddTransient<IRepository<JBank, int>, JBankRepository>();
            services.AddTransient<IRepository<JNegeri, int>, JNegeriRepository>();
            services.AddTransient<IRepository<AkCarta, int>, AkCartaRepository>();
            services.AddTransient<IRepository<AkAkaun, int>, AkAkaunRepository>();
            services.AddTransient<IRepository<AkTerima, int>, AkTerimaRepository>();
            services.AddTransient<ListViewIRepository<AkTerima1, int>, AkTerima1Repository>();
            services.AddTransient<ListViewIRepository<AkTerima2, int>, AkTerima2Repository>();
            services.AddTransient<IRepository<AkPO, int>, AkPORepository>();
            services.AddTransient<ListViewIRepository<AkPO1, int>, AkPO1Repository>();
            services.AddTransient<ListViewIRepository<AkPO2, int>, AkPO2Repository>();
            services.AddTransient<IRepository<AkPembekal, int>, AkPembekalRepository>();
            services.AddTransient<IRepository<AkJurnal, int>, AkJurnalRepository>();
            services.AddTransient<ListViewIRepository<AkJurnal1, int>, AkJurnal1Repository>();
            services.AddTransient<IRepository<AkBelian, int>, AkBelianRepository>();
            services.AddTransient<ListViewIRepository<AkBelian1, int>, AkBelian1Repository>();
            services.AddTransient<ListViewIRepository<AkBelian2, int>, AkBelian2Repository>();
            services.AddTransient<AppLogIRepository<AppLog, int>, AppLogRepository>();
            services.AddTransient<IRepository<AkPV, int>, AkPVRepository>();
            services.AddTransient<ListViewIRepository<AkPV1, int>, AkPV1Repository>();
            services.AddTransient<ListViewIRepository<AkPV2, int>, AkPV2Repository>();
            services.AddTransient<IRepository<SuPekerja, int>, SuPekerjaRepository>();
            services.AddTransient<ListViewIRepository<SuTanggunganPekerja, int>, SuTanggunganPekerjaRepository>();
            services.AddTransient<IRepository<JAgama, int>, JAgamaRepository>();
            services.AddTransient<IRepository<JBangsa, int>, JBangsaRepository>();
            services.AddTransient<IRepository<JJawatanPekerja, int>, JJawatanPekerjaRepository>();
            services.AddTransient<IRepository<JCaraBayar, int>, JCaraBayarRepository>();
            services.AddTransient<IRepository<AbBukuVot, int>, AbBukuVotRepository>();
            services.AddTransient<IRepository<JJantina, int>, JJantinaRepository>();
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
