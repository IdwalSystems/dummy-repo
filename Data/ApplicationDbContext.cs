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
        public DbSet<JKW> JKW { get; set; }

        public DbSet<JCaraBayar> JCaraBayar { get; set; }

        public DbSet<SiModul> SiModul { get; set; }

        public DbSet<JBank> JBank { get; set; }
        public DbSet<JNegeri> JNegeri { get; set; }
        public DbSet<AkBank> AkBank { get; set; }
        public DbSet<AkCarta> AkCarta { get; set; }
        public DbSet<JJenis> JJenis { get; set; }
        public DbSet<JParas> JParas { get; set; }
        public DbSet<AkAkaun> AkAkaun { get; set; }
        public DbSet<AkTerima> AkTerima { get; set; }
        public DbSet<AkTerima1> AkTerima1 { get; set; }
        public DbSet<AkTerima2> AkTerima2 { get; set; }
        public DbSet<AkPembekal> AkPembekal { get; set; }
        public DbSet<AkPO> AkPO { get; set; }
        public DbSet<AkPO1> AkPO1 { get; set; }
        public DbSet<AkPO2> AkPO2 { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AkBank>()
                .HasOne(e => e.JBank)
                .WithMany(c => c.AkBank)
                ;

            modelBuilder.Entity<AkBank>()
                .HasOne(e => e.JKW)
                .WithMany(c => c.AkBank)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AkCarta>()
                .HasOne(e => e.JKW)
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
                    .HasOne(m => m.JKW)
                    .WithMany(t => t.AkTerima)
                    .HasForeignKey(m => m.JKWId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired();

            modelBuilder.Entity<AkTerima>()
                    .HasOne(m => m.JNegeri)
                    .WithMany(t => t.AkTerima)
                    .HasForeignKey(m => m.JNegeriId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired();

            modelBuilder.Entity<AkPO>()
                    .HasOne(m => m.AkPembekal)
                    .WithMany(t => t.AkPO)
                    .HasForeignKey(m => m.AkPembekalId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired();

            modelBuilder.Entity<AkPO2>()
                    .HasOne(m => m.JKW)
                    .WithMany(t => t.AkPO2)
                    .HasForeignKey(m => m.JKWId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired();

            modelBuilder.Entity<AkPO2>()
                    .HasOne(m => m.AkPO)
                    .WithMany(t => t.AkPO2)
                    .HasForeignKey(m => m.AkPOId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired();

        }
    }
    
}
