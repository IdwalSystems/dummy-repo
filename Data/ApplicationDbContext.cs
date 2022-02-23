using Microsoft.AspNetCore.Identity;
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
        public DbSet<AkPOLaras> AkPOLaras { get; set; }
        public DbSet<AkPOLaras1> AkPOLaras1 { get; set; }
        public DbSet<AkPOLaras2> AkPOLaras2 { get; set; }
        public DbSet<AkJurnal> AkJurnal { get; set; }
        public DbSet<AkJurnal1> AkJurnal1 { get; set; }
        public DbSet<AppLog> AppLog { get; set; }
        public DbSet<AkBelian> AkBelian { get; set; }
        public DbSet<AkBelian1> AkBelian1 { get; set; }
        public DbSet<AkBelian2> AkBelian2 { get; set; }
        public DbSet<AkPV> AkPV { get; set; }
        public DbSet<AkPV1> AkPV1 { get; set; }
        public DbSet<AkPV2> AkPV2 { get; set; }
        public DbSet<SuPekerja> SuPekerja { get; set; }
        public DbSet<SuTanggunganPekerja> SuTanggunganPekerja { get; set; }
        public DbSet<JJawatanPekerja>JJawatanPekerja { get; set; }
        public DbSet<JBangsa> JBangsa { get; set; }
        public DbSet<JAgama> JAgama { get; set; }
        public DbSet<AbBukuVot> AbBukuVot { get; set; }
        public DbSet<JSukan> JSukan { get; set; }
        public DbSet<JTahapAktiviti> JTahapAktiviti { get; set; }
        public DbSet<SpPendahuluanPelbagai> SpPendahuluanPelbagai { get; set; }
        public DbSet<SpPendahuluanPelbagai1> SpPendahuluanPelbagai1 { get; set; }
        public DbSet<SpPendahuluanPelbagai2> SpPendahuluanPelbagai2 { get; set; }
        public DbSet<JJantina> JJantina { get; set; }
        public DbSet<AkTunaiRuncit> AkTunaiRuncit { get; set; }
        public DbSet<AkTunaiPemegang> AkTunaiPemegang { get; set; }
        public DbSet<AkTunaiCV> AkTunaiCV { get; set; }
        public DbSet<AkTunaiCV1> AkTunaiCV1 { get; set; }
        public DbSet<AkTunaiLejar> AkTunaiLejar { get; set; }
        public DbSet<AkNotaMinta> AkNotaMinta { get; set; }
        public DbSet<AkNotaMinta1> AkNotaMinta1 { get; set; }
        public DbSet<AkNotaMinta2> AkNotaMinta2 { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //load item without soft delete
            modelBuilder.Entity<JKW>().HasQueryFilter(m => EF.Property<int>(m, "FlHapus") == 0);
            modelBuilder.Entity<JBahagian>().HasQueryFilter(m => EF.Property<int>(m, "FlHapus") == 0);
            modelBuilder.Entity<JCaraBayar>().HasQueryFilter(m => EF.Property<int>(m, "FlHapus") == 0);
            modelBuilder.Entity<AkBank>().HasQueryFilter(m => EF.Property<int>(m, "FlHapus") == 0);
            modelBuilder.Entity<JBank>().HasQueryFilter(m => EF.Property<int>(m, "FlHapus") == 0);
            modelBuilder.Entity<JNegeri>().HasQueryFilter(m => EF.Property<int>(m, "FlHapus") == 0);
            modelBuilder.Entity<JAgama>().HasQueryFilter(m => EF.Property<int>(m, "FlHapus") == 0);
            modelBuilder.Entity<JBangsa>().HasQueryFilter(m => EF.Property<int>(m, "FlHapus") == 0);
            modelBuilder.Entity<JSukan>().HasQueryFilter(m => EF.Property<int>(m, "FlHapus") == 0);
            modelBuilder.Entity<JTahapAktiviti>().HasQueryFilter(m => EF.Property<int>(m, "FlHapus") == 0);
            modelBuilder.Entity<JJantina>().HasQueryFilter(m => EF.Property<int>(m, "FlHapus") == 0);
            modelBuilder.Entity<JJawatanPekerja>().HasQueryFilter(m => EF.Property<int>(m, "FlHapus") == 0);
            modelBuilder.Entity<AkPembekal>().HasQueryFilter(m => EF.Property<int>(m, "FlHapus") == 0);
            modelBuilder.Entity<SuPekerja>().HasQueryFilter(m => EF.Property<int>(m, "FlHapus") == 0);
            modelBuilder.Entity<AkCarta>().HasQueryFilter(m => EF.Property<int>(m, "FlHapus") == 0);
            modelBuilder.Entity<AkTunaiPemegang>().HasQueryFilter(m => EF.Property<int>(m, "FlHapus") == 0);
            modelBuilder.Entity<JJenis>().HasQueryFilter(m => EF.Property<int>(m, "FlHapus") == 0);
            modelBuilder.Entity<JParas>().HasQueryFilter(m => EF.Property<int>(m, "FlHapus") == 0);

            //Tanggungan
            modelBuilder.Entity<AkBelian>().HasQueryFilter(m => EF.Property<int>(m, "FlHapus") == 0);

            //Tanggungan End
            //load item without soft delete end

            //modelBuilder.Entity<IdentityRole>()
            //    .HasData(new IdentityRole { Name = "Admin", NormalizedName = "Admin".ToUpper() 
            //    });
            //modelBuilder.Entity<IdentityRole>()
            //    .HasData(
            //        new IdentityRole { Name = "Admin", NormalizedName = "Admin".ToUpper() },
            //        new IdentityRole { Name = "Supervisor", NormalizedName = "Supervisor".ToUpper() },
            //        new IdentityRole { Name = "User", NormalizedName = "User".ToUpper() }
            //    );

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
                .WithMany(c => c.AkCarta)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AkAkaun>()
                    .HasOne(m => m.AkCarta1)
                    .WithMany(t => t.AkAkaun1)
                    .HasForeignKey(m => m.AkCartaId1)
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired();

            modelBuilder.Entity<AkAkaun>()
                    .HasOne(m => m.AkCarta2!)
                    .WithMany(t => t.AkAkaun2)
                    .HasForeignKey(m => m.AkCartaId2)
                    .OnDelete(DeleteBehavior.Restrict);
            //AkTerima
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

            modelBuilder.Entity<AkTerima>()
                    .HasOne(m => m.AkBank)
                    .WithMany(t => t.AkTerima)
                    .HasForeignKey(m => m.AkBankId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired();

            modelBuilder.Entity<AkTerima>()
               .HasOne(m => m.SpPendahuluanPelbagai!)
               .WithMany(t => t.AkTerima)
               .HasForeignKey(m => m.SpPendahuluanPelbagaiId)
               .OnDelete(DeleteBehavior.NoAction);
            //AkTerima end
            //AkPO
            modelBuilder.Entity<AkPO>()
                    .HasOne(m => m.AkPembekal)
                    .WithMany(t => t.AkPO)
                    .HasForeignKey(m => m.AkPembekalId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired();

            modelBuilder.Entity<AkPO>()
                    .HasOne(m => m.JKW)
                    .WithMany(t => t.AkPO)
                    .HasForeignKey(m => m.JKWId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired();

            modelBuilder.Entity<AkPO>()
                .HasOne(m => m.AkNotaMinta)
                .WithMany(t => t.AkPO)
                .HasForeignKey(m => m.AkNotaMintaId)
                .OnDelete(DeleteBehavior.NoAction);
            //AkPO end
            //AkPO
            modelBuilder.Entity<AkPOLaras>()
                    .HasOne(m => m.AkPO)
                    .WithMany(t => t.AkPOLaras)
                    .HasForeignKey(m => m.AkPOId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired();

            modelBuilder.Entity<AkPOLaras>()
                    .HasOne(m => m.JKW)
                    .WithMany(t => t.AkPOLaras)
                    .HasForeignKey(m => m.JKWId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired();
            //AkPO end
            //AkNotaMinta
            modelBuilder.Entity<AkNotaMinta>()
                    .HasOne(m => m.AkPembekal)
                    .WithMany(t => t.AkNotaMinta)
                    .HasForeignKey(m => m.AkPembekalId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired();

            modelBuilder.Entity<AkNotaMinta>()
                    .HasOne(m => m.JKW)
                    .WithMany(t => t.AkNotaMinta)
                    .HasForeignKey(m => m.JKWId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired();
            //AkNotaMinta end
            //AkJurnal
            modelBuilder.Entity<AkJurnal>()
                .HasOne(m => m.JKW)
                .WithMany(t => t.AkJurnal)
                .HasForeignKey(m => m.JKWId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
            //AkJurnal end
            //AkBelian
            modelBuilder.Entity<AkBelian>()
                .HasOne(m => m.JKW)
                .WithMany(t => t.AkBelian)
                .HasForeignKey(m => m.JKWId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            modelBuilder.Entity<AkBelian>()
                .HasOne(m => m.AkPO!)
                .WithMany(t => t.AkBelian)
                .HasForeignKey(m => m.AkPOId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<AkBelian>()
                .HasOne(m => m.AkPembekal)
                .WithMany(t => t.AkBelian)
                .HasForeignKey(m => m.AkPembekalId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            modelBuilder.Entity<AkBelian>()
                    .HasOne(m => m.KodObjekAP)
                    .WithMany(t => t.KodObjekAP)
                    .HasForeignKey(m => m.KodObjekAPId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired();
            //AkBelian end
            //AkPV
            modelBuilder.Entity<AkPV>()
                .HasOne(m => m.JKW)
                .WithMany(t => t.AkPV)
                .HasForeignKey(m => m.JKWId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            modelBuilder.Entity<AkPV2>()
                .HasOne(m => m.AkBelian!)
                .WithMany(t => t.AkPV2)
                .HasForeignKey(m => m.AkBelianId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<AkPV>()
                .HasOne(m => m.AkPembekal!)
                .WithMany(t => t.AkPV)
                .HasForeignKey(m => m.AkPembekalId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<AkPV>()
                .HasOne(m => m.SuPekerja!)
                .WithMany(t => t.AkPV)
                .HasForeignKey(m => m.SuPekerjaId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<AkPV>()
                    .HasOne(m => m.AkBank)
                    .WithMany(t => t.AkPV)
                    .HasForeignKey(m => m.AkBankId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired();

            modelBuilder.Entity<AkPV>()
                .HasOne(m => m.AkTunaiRuncit!)
                .WithMany(t => t.AkPV)
                .HasForeignKey(m => m.AkTunaiRuncitId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<AkPV>()
                .HasOne(m => m.SpPendahuluanPelbagai!)
                .WithMany(t => t.AkPV)
                .HasForeignKey(m => m.SpPendahuluanPelbagaiId)
                .OnDelete(DeleteBehavior.NoAction);
            //AKPV end
            //AkTunaiRuncit
            modelBuilder.Entity<AkTunaiRuncit>()
                .HasOne(m => m.JKW)
                .WithMany(t => t.AkTunaiRuncit)
                .HasForeignKey(m => m.JKWId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            modelBuilder.Entity<AkTunaiRuncit>()
                    .HasOne(m => m.AkCarta)
                    .WithMany(t => t.AkTunaiRuncit)
                    .HasForeignKey(m => m.AkCartaId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired();
            //AkTunaiRuncit end
            //AkTunaiCV
            modelBuilder.Entity<AkTunaiCV>()
                .HasOne(m => m.AkPembekal!)
                .WithMany(t => t.AkTunaiCV)
                .HasForeignKey(m => m.AkPembekalId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<AkTunaiCV>()
                .HasOne(m => m.SuPekerja!)
                .WithMany(t => t.AkTunaiCV)
                .HasForeignKey(m => m.SuPekerjaId)
                .OnDelete(DeleteBehavior.NoAction);
            //AkTunaiCV end

            //set default value
            modelBuilder.Entity<AkJurnal>().Property(b => b.Catatan1).HasDefaultValue("");
            modelBuilder.Entity<AkJurnal>().Property(b => b.Catatan2).HasDefaultValue("");
            modelBuilder.Entity<AkJurnal>().Property(b => b.Catatan3).HasDefaultValue("");
            modelBuilder.Entity<AkJurnal>().Property(b => b.Catatan4).HasDefaultValue("");
        }
        public DbSet<MSNK.Models.Modules.JBahagian> JBahagian { get; set; }
    }
}
