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
        public DbSet<Jenis> Jenis { get; set; }
        public DbSet<Paras> Paras { get; set; }
        public DbSet<AkAkaun> AkAkaun { get; set; }
        public DbSet<AkTerima> AkTerima { get; set; }
        public DbSet<AkTerima1> AkTerima1 { get; set; }
        public DbSet<AkTerima2> AkTerima2 { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AkBank>()
                .HasOne(e => e.Bank)
                .WithMany(c => c.AkBank)
                ;

            modelBuilder.Entity<AkBank>()
                .HasOne(e => e.KW)
                .WithMany(c => c.AkBank)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AkCarta>()
                .HasOne(e => e.KW)
                .WithMany(c => c.AkCarta);

            modelBuilder.Entity<AkAkaun>()
                    .HasOne(m => m.AkCarta1)
                    .WithMany(t => t.AkAkaun1)
                    .HasForeignKey(m => m.AkCartaId1)
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired();

            modelBuilder.Entity<AkAkaun>()
                    .HasOne(m => m.AkCarta2)
                    .WithMany(t => t.AkAkaun2)
                    .HasForeignKey(m => m.AkCartaId2)
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired();

            modelBuilder.Entity<AkTerima1>()
                    .HasOne(m => m.AkTerima)
                    .WithMany(t => t.AkTerima1)
                    .HasForeignKey(m => m.AkTerimaId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired();

            modelBuilder.Entity<AkTerima2>()
                    .HasOne(m => m.AkTerima)
                    .WithMany(t => t.AkTerima2)
                    .HasForeignKey(m => m.AkTerimaId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired();

            modelBuilder.Entity<AkTerima>()
                    .HasOne(m => m.KW)
                    .WithMany(t => t.AkTerima)
                    .HasForeignKey(m => m.KWId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired();

            modelBuilder.Entity<AkTerima>()
                    .HasOne(m => m.Negeri)
                    .WithMany(t => t.AkTerima)
                    .HasForeignKey(m => m.NegeriId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired();

        }
    }
    
}
