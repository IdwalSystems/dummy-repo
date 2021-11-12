using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MSNK.Models.Administration;
using MSNK.Models.Modules;

namespace MSNK.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions options): base(options)
        {

        }

        public DbSet<ApplicationUser> applicationUsers { get; set; }

        //module
        public DbSet<KW> KW { get; set; }

        public DbSet<CaraBayar> CaraBayar { get; set; }

        public DbSet<Modul> Modul { get; set; }

        public DbSet<Bank> Bank { get; set; }
        public DbSet<Negeri> Negeri { get; set; }
        public DbSet<AkBank> AkBank { get; set; }
        public DbSet<AkCarta> AkCarta { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AkBank>()
                .HasOne(e => e.Bank)
                .WithMany(c => c.AkBank);

            modelBuilder.Entity<AkBank>()
                .HasOne(e => e.KW)
                .WithMany(c => c.AkBank);

            modelBuilder.Entity<AkCarta>()
                .HasOne(e => e.KW)
                .WithMany(c => c.AkCarta);


        }
    }
    
}
