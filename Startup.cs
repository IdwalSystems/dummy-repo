using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MSNK.Data;
using MSNK.Models.Modules;
using MSNK.Models.Modules.Cart.Session;
using MSNK.Models.Modules.EFRepository;
using MSNK.Models.Modules.IRepository;
using Rotativa.AspNetCore;
using System;

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
                opt.ExpireTimeSpan = TimeSpan.FromSeconds(300);
                opt.LoginPath = "/Account/Login";
                opt.SlidingExpiration = true;
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
            services.AddTransient<IRepository<AkTunaiRuncit, int, string>, AkTunaiRuncitRepository>();
            services.AddTransient<IRepository<AkTunaiCV, int, string>, AkTunaiCVRepository>();
            services.AddScoped(ss => SessionCartTerima.GetCart(ss));
            services.AddScoped(ss => SessionCartPO.GetCart(ss));
            services.AddScoped(ss => SessionCartJurnal.GetCart(ss));
            services.AddScoped(ss => SessionCartBelian.GetCart(ss));
            services.AddScoped(ss => SessionCartPV.GetCart(ss));
            services.AddScoped(ss => SessionCartPekerja.GetCart(ss));
            services.AddScoped(ss => SessionCartTunaiRuncit.GetCart(ss));
            services.AddScoped(ss => SessionCartTunaiCV.GetCart(ss));

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddControllersWithViews()
                .AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );

            services.AddAuthorization(options=>
            {
                //Menu Terimaan
                //Resit Rasmi
                options.AddPolicy("PR001", policy => policy.RequireClaim("PR001"));
                options.AddPolicy("PR001C", policy => policy.RequireClaim("PR001C"));
                options.AddPolicy("PR001E", policy => policy.RequireClaim("PR001E"));
                options.AddPolicy("PR001D", policy => policy.RequireClaim("PR001D"));
                options.AddPolicy("PR001P", policy => policy.RequireClaim("PR001P"));
                options.AddPolicy("PR001B", policy => policy.RequireClaim("PR001B"));
                options.AddPolicy("PR001T", policy => policy.RequireClaim("PR001T"));
                options.AddPolicy("PR001UT", policy => policy.RequireClaim("PR001UT"));
                //Resit Rasmi end
                //Menu Tanggungan
                //Pesanan Tempatan
                options.AddPolicy("TG001", policy => policy.RequireClaim("TG001"));
                options.AddPolicy("TG001C", policy => policy.RequireClaim("TG001C"));
                options.AddPolicy("TG001E", policy => policy.RequireClaim("TG001E"));
                options.AddPolicy("TG001D", policy => policy.RequireClaim("TG001D"));
                options.AddPolicy("TG001P", policy => policy.RequireClaim("TG001P"));
                options.AddPolicy("TG001B", policy => policy.RequireClaim("TG001B"));
                options.AddPolicy("TG001T", policy => policy.RequireClaim("TG001T"));
                options.AddPolicy("TG001UT", policy => policy.RequireClaim("TG001UT"));
                //Pesanan Tempatan End
                //Invois Pembekal
                options.AddPolicy("TG002", policy => policy.RequireClaim("TG002"));
                options.AddPolicy("TG002C", policy => policy.RequireClaim("TG002C"));
                options.AddPolicy("TG002E", policy => policy.RequireClaim("TG002E"));
                options.AddPolicy("TG002D", policy => policy.RequireClaim("TG002D"));
                //options.AddPolicy("TG002P", policy => policy.RequireClaim("TG002P"));
                options.AddPolicy("TG002B", policy => policy.RequireClaim("TG002B"));
                options.AddPolicy("TG002T", policy => policy.RequireClaim("TG002T"));
                options.AddPolicy("TG002UT", policy => policy.RequireClaim("TG002UT"));
                //Invois Pembekal End
                //Menu Baucer
                //Baucer Pembayaran
                options.AddPolicy("PV001", policy => policy.RequireClaim("PV001"));
                options.AddPolicy("PV001C", policy => policy.RequireClaim("PV001C"));
                options.AddPolicy("PV001E", policy => policy.RequireClaim("PV001E"));
                options.AddPolicy("PV001D", policy => policy.RequireClaim("PV001D"));
                options.AddPolicy("PV001P", policy => policy.RequireClaim("PV001P"));
                options.AddPolicy("PV001B", policy => policy.RequireClaim("PV001B"));
                options.AddPolicy("PV001T", policy => policy.RequireClaim("PV001T"));
                options.AddPolicy("PV001UT", policy => policy.RequireClaim("PV001UT"));
                //Baucer Pembayaran End
                //Baucer Jurnal
                options.AddPolicy("JU001", policy => policy.RequireClaim("JU001"));
                options.AddPolicy("JU001C", policy => policy.RequireClaim("JU001C"));
                options.AddPolicy("JU001E", policy => policy.RequireClaim("JU001E"));
                options.AddPolicy("JU001D", policy => policy.RequireClaim("JU001D"));
                options.AddPolicy("JU001P", policy => policy.RequireClaim("JU001P"));
                options.AddPolicy("JU001B", policy => policy.RequireClaim("JU001B"));
                options.AddPolicy("JU001T", policy => policy.RequireClaim("JU001T"));
                options.AddPolicy("JU001UT", policy => policy.RequireClaim("JU001UT"));
                //Baucer Jurnal End
                //Menu Tunai Runcit
                //Pemegang Tunai Runcit
                options.AddPolicy("TR001", policy => policy.RequireClaim("TR001"));
                options.AddPolicy("TR001C", policy => policy.RequireClaim("TR001C"));
                options.AddPolicy("TR001E", policy => policy.RequireClaim("TR001E"));
                options.AddPolicy("TR001D", policy => policy.RequireClaim("TR001D"));
                options.AddPolicy("TR001P", policy => policy.RequireClaim("TR001P"));
                //options.AddPolicy("TR001B", policy => policy.RequireClaim("TR001B"));
                //options.AddPolicy("TR001T", policy => policy.RequireClaim("TR001T"));
                //options.AddPolicy("TR001UT", policy => policy.RequireClaim("TR001UT"));
                //Pemegang Tunai Runcit End
                //Tunai Keluar
                options.AddPolicy("TR002", policy => policy.RequireClaim("TR002"));
                options.AddPolicy("TR002C", policy => policy.RequireClaim("TR002C"));
                options.AddPolicy("TR002E", policy => policy.RequireClaim("TR002E"));
                options.AddPolicy("TR002D", policy => policy.RequireClaim("TR002D"));
                options.AddPolicy("TR002P", policy => policy.RequireClaim("TR002P"));
                options.AddPolicy("TR002B", policy => policy.RequireClaim("TR002B"));
                options.AddPolicy("TR002T", policy => policy.RequireClaim("TR002T"));
                options.AddPolicy("TR002UT", policy => policy.RequireClaim("TR002UT"));
                //Tunai Keluar End
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
