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
        public DbSet<ExceptionLogger> ExceptionLogger { get; set; }
        public DbSet<HubConnection> HubConnection { get; set; }
        public DbSet<Notification> Notification { get; set; }

        //module
        public DbSet<JKW> JKW { get; set; }
        public DbSet<JCaraBayar> JCaraBayar { get; set; }
        public DbSet<SiModul> SiModul { get; set; }
        public DbSet<JBank> JBank { get; set; }
        public DbSet<JPTJ> JPTJ { get; set; }
        public DbSet<JNegeri> JNegeri { get; set; }
        public DbSet<AkBank> AkBank { get; set; }
        public DbSet<AkCarta> AkCarta { get; set; }
        public DbSet<JJenis> JJenis { get; set; }
        public DbSet<JParas> JParas { get; set; }
        public DbSet<AkAkaun> AkAkaun { get; set; }
        public DbSet<AkTerima> AkTerima { get; set; }
        public DbSet<AkTerima1> AkTerima1 { get; set; }
        public DbSet<AkTerima2> AkTerima2 { get; set; }
        public DbSet<AkTerima3> AkTerima3 { get; set; }
        public DbSet<AkPembekal> AkPembekal { get; set; }
        public DbSet<AkPO> AkPO { get; set; }
        public DbSet<AkPO1> AkPO1 { get; set; }
        public DbSet<AkPO2> AkPO2 { get; set; }
        public DbSet<AkInden> AkInden { get; set; }
        public DbSet<AkInden1> AkInden1 { get; set; }
        public DbSet<AkInden2> AkInden2 { get; set; }
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
        public DbSet<AkPVGanda> AkPVGanda { get; set; }
        public DbSet<SuPekerja> SuPekerja { get; set; }
        public DbSet<SuTanggunganPekerja> SuTanggunganPekerja { get; set; }
        public DbSet<SuAtlet> SuAtlet { get; set; }
        public DbSet<SuJurulatih> SuJurulatih { get; set; }
        public DbSet<SuProfil> SuProfil { get; set; }
        public DbSet<SuProfil1> SuProfil1 { get; set; }
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
        public DbSet<JBahagian> JBahagian { get; set; }
        public DbSet<AbWaran> AbWaran { get; set; }
        public DbSet<AbWaran1> AbWaran1 { get; set; }
        public DbSet<JPelulus> JPelulus { get; set; }
        public DbSet<JPenyemak> JPenyemak { get; set; }
        public DbSet<JProfilKategori> JProfilKategori { get; set; }
        public DbSet<SiAppInfo> SiAppInfo { get; set; }
        public DbSet<AkCimbEFT> AkCimbEFT { get; set; }
        public DbSet<AkCimbEFT1> AkCimbEFT1 { get; set; }
        public DbSet<AkPenghutang> AkPenghutang { get; set; }
        public DbSet<AkInvois> AkInvois { get; set; }
        public DbSet<AkInvois1> AkInvois1 { get; set; }
        public DbSet<AkInvois2> AkInvois2 { get; set; }
        public DbSet<AkPenyataPemungut> AkPenyataPemungut { get; set; }
        public DbSet<AkPenyataPemungut1> AkPenyataPemungut1 { get; set; }
        public DbSet<AkPenyataPemungut2> AkPenyataPemungut2 { get; set; }
        public DbSet<AkBankRecon> AkBankRecon { get; set; }
        public DbSet<AkBankReconPenyataBank> AkBankReconPenyataBank { get; set; }
        public DbSet<AkPadananPenyata> AkPadananPenyata { get; set; }
        public DbSet<AkNotaDebitKreditBelian> AkNotaDebitKreditBelian { get; set; }
        public DbSet<AkNotaDebitKreditBelian1> AkNotaDebitKreditBelian1 { get; set; }
        public DbSet<AkNotaDebitKreditBelian2> AkNotaDebitKreditBelian2 { get; set; }

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
            modelBuilder.Entity<AkPembekal>().HasQueryFilter(m => EF.Property<int>(m, "FlHapus") == 0);
            modelBuilder.Entity<SuPekerja>().HasQueryFilter(m => EF.Property<int>(m, "FlHapus") == 0);
            modelBuilder.Entity<AkCarta>().HasQueryFilter(m => EF.Property<int>(m, "FlHapus") == 0);
            modelBuilder.Entity<AkTunaiPemegang>().HasQueryFilter(m => EF.Property<int>(m, "FlHapus") == 0);
            modelBuilder.Entity<JJenis>().HasQueryFilter(m => EF.Property<int>(m, "FlHapus") == 0);
            modelBuilder.Entity<JParas>().HasQueryFilter(m => EF.Property<int>(m, "FlHapus") == 0);
            modelBuilder.Entity<JPelulus>().HasQueryFilter(m => EF.Property<int>(m, "FlHapus") == 0);
            modelBuilder.Entity<JPenyemak>().HasQueryFilter(m => EF.Property<int>(m, "FlHapus") == 0);
            modelBuilder.Entity<JProfilKategori>().HasQueryFilter(m => EF.Property<int>(m, "FlHapus") == 0);
            modelBuilder.Entity<AkPenghutang>().HasQueryFilter(m => EF.Property<int>(m, "FlHapus") == 0);

            //Terimaan
            modelBuilder.Entity<AkTerima>().HasQueryFilter(m => EF.Property<int>(m, "FlHapus") == 0);
            modelBuilder.Entity<AkPenyataPemungut>().HasQueryFilter(m => EF.Property<int>(m, "FlHapus") == 0);
            // Terimaan End

            //Baucer
            modelBuilder.Entity<AkPV>().HasQueryFilter(m => EF.Property<int>(m, "FlHapus") == 0);
            modelBuilder.Entity<AkJurnal>().HasQueryFilter(m => EF.Property<int>(m, "FlHapus") == 0);
            // Baucer End
            //Tanggungan
            modelBuilder.Entity<AkNotaMinta>().HasQueryFilter(m => EF.Property<int>(m, "FlHapus") == 0);
            modelBuilder.Entity<AkPO>().HasQueryFilter(m => EF.Property<int>(m, "FlHapus") == 0);
            modelBuilder.Entity<AkInden>().HasQueryFilter(m => EF.Property<int>(m, "FlHapus") == 0);
            modelBuilder.Entity<AkPOLaras>().HasQueryFilter(m => EF.Property<int>(m, "FlHapus") == 0);
            modelBuilder.Entity<AkBelian>().HasQueryFilter(m => EF.Property<int>(m, "FlHapus") == 0);
            //Tanggungan End
            //Tunai Runcit
            modelBuilder.Entity<AkTunaiCV>().HasQueryFilter(m => EF.Property<int>(m, "FlHapus") == 0);
            //Tunai Runcit End
            //Permohonan
            modelBuilder.Entity<SpPendahuluanPelbagai>().HasQueryFilter(m => EF.Property<int>(m, "FlHapus") == 0);
            //Permohonan End
            //Belanjawan
            modelBuilder.Entity<AbWaran>().HasQueryFilter(m => EF.Property<int>(m, "FlHapus") == 0);
            //Belanjawan end
            //Profil Atlet & Jurulatih
            modelBuilder.Entity<SuProfil>().HasQueryFilter(m => EF.Property<int>(m, "FlHapus") == 0);
            //Profil Atlet & Jurulatih end
            //Invois
            modelBuilder.Entity<AkInvois>().HasQueryFilter(m => EF.Property<int>(m, "FlHapus") == 0);
            //Invois end
            //Nota Debit Kredit Belian
            modelBuilder.Entity<AkNotaDebitKreditBelian>().HasQueryFilter(m => EF.Property<int>(m, "FlHapus") == 0);
            //Nota Debit Kredit Belian end
            //BankRecon
            modelBuilder.Entity<AkBankRecon>().HasQueryFilter(m => EF.Property<int>(m, "FlHapus") == 0);
            //BankRecon end

            //default bool to true

            //default bool to true end
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

            //AbWaran
            modelBuilder.Entity<AbWaran>()
                .HasOne(e => e.JKW)
                .WithMany(c => c.AbWaran)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AbWaran>()
                .HasOne(e => e.JBahagian)
                .WithMany(c => c.AbWaran)
                .OnDelete(DeleteBehavior.Restrict);
            //AbWaran End

            //AbBukuVot
            modelBuilder.Entity<AbBukuVot>()
                .HasOne(e => e.JBahagian)
                .WithMany(c => c.abBukuVot)
                .HasForeignKey(m => m.JBahagianId)
                .OnDelete(DeleteBehavior.Restrict);
            //AbBukuVot end

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

            modelBuilder.Entity<AkAkaun>()
                    .HasOne(m => m.JBahagian)
                    .WithMany(t => t.AkAkaun)
                    .HasForeignKey(m => m.JBahagianId)
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

            modelBuilder.Entity<AkTerima3>()
                .HasOne(m => m.AkInvois)
                .WithMany(t => t.AkTerima3)
                .HasForeignKey(m => m.AkInvoisId)
                .OnDelete(DeleteBehavior.ClientNoAction);
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

            modelBuilder.Entity<AkPO>()
                    .HasOne(m => m.JBahagian)
                    .WithMany(t => t.AkPO)
                    .HasForeignKey(m => m.JBahagianId)
                    .OnDelete(DeleteBehavior.Restrict);
            //AkPO end
            //AkInden
            modelBuilder.Entity<AkInden>()
                    .HasOne(m => m.AkPembekal)
                    .WithMany(t => t.AkInden)
                    .HasForeignKey(m => m.AkPembekalId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired();

            modelBuilder.Entity<AkInden>()
                    .HasOne(m => m.JKW)
                    .WithMany(t => t.AkInden)
                    .HasForeignKey(m => m.JKWId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired();

            modelBuilder.Entity<AkInden>()
                .HasOne(m => m.AkNotaMinta)
                .WithMany(t => t.AkInden)
                .HasForeignKey(m => m.AkNotaMintaId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<AkInden>()
                    .HasOne(m => m.JBahagian)
                    .WithMany(t => t.AkInden)
                    .HasForeignKey(m => m.JBahagianId)
                    .OnDelete(DeleteBehavior.Restrict);
            //AkInden end
            //AkPOLaras
            modelBuilder.Entity<AkPOLaras>()
                    .HasOne(m => m.AkPO)
                    .WithMany(t => t.AkPOLaras)
                    .HasForeignKey(m => m.AkPOId)
                    .OnDelete(DeleteBehavior.ClientNoAction)
                    .IsRequired();

            modelBuilder.Entity<AkPOLaras>()
                    .HasOne(m => m.JKW)
                    .WithMany(t => t.AkPOLaras)
                    .HasForeignKey(m => m.JKWId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired();
            //AkPOLaras end
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

            modelBuilder.Entity<AkNotaMinta>()
                    .HasOne(m => m.JBahagian)
                    .WithMany(t => t.AkNotaMinta)
                    .HasForeignKey(m => m.JBahagianId)
                    .OnDelete(DeleteBehavior.Restrict);
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

                // Temporal Table
                modelBuilder.Entity<AkBelian>().Property(x => x.ValidFromUTC)
                            .IsRequired()
                            .ValueGeneratedOnAddOrUpdate()
                            .HasDefaultValueSql("SYSUTCDATETIME()");

                modelBuilder.Entity<AkBelian>().Property(x => x.ValidToUTC)
                            .IsRequired()
                            .ValueGeneratedOnAddOrUpdate()
                            .HasDefaultValueSql("CONVERT(DATETIME2, '9999-12-31 23:59:59.9999999')");
                //Temporal Table end

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
                .OnDelete(DeleteBehavior.ClientNoAction);

            modelBuilder.Entity<AkBelian>()
                .HasOne(m => m.AkPembekal)
                .WithMany(t => t.AkBelian)
                .HasForeignKey(m => m.AkPembekalId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            modelBuilder.Entity<AkBelian>()
                    .HasOne(m => m.KodObjekAP)
                    .WithMany(t => t.AkBelian)
                    .HasForeignKey(m => m.KodObjekAPId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired();

            modelBuilder.Entity<AkBelian>()
                    .HasOne(m => m.JBahagian)
                    .WithMany(t => t.AkBelian)
                    .HasForeignKey(m => m.JBahagianId)
                    .OnDelete(DeleteBehavior.Restrict);
            //AkBelian end

            //AkInvois
            modelBuilder.Entity<AkInvois>()
                .HasOne(m => m.JKW)
                .WithMany(t => t.AkInvois)
                .HasForeignKey(m => m.JKWId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            modelBuilder.Entity<AkInvois>()
                .HasOne(m => m.AkPenghutang)
                .WithMany(t => t.AkInvois)
                .HasForeignKey(m => m.AkPenghutangId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            modelBuilder.Entity<AkInvois>()
                    .HasOne(m => m.KodObjekAP)
                    .WithMany(t => t.AkInvois)
                    .HasForeignKey(m => m.KodObjekAPId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired();

            modelBuilder.Entity<AkInvois>()
                    .HasOne(m => m.JBahagian)
                    .WithMany(t => t.AkInvois)
                    .HasForeignKey(m => m.JBahagianId)
                    .OnDelete(DeleteBehavior.Restrict);
            //AkInvois end

            //AkNotaDebitKreditBelian
            modelBuilder.Entity<AkNotaDebitKreditBelian>()
                .HasOne(m => m.JBahagian)
                .WithMany(m => m.AkNotaDebitKreditBelian)
                .HasForeignKey(m => m.JBahagianId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            modelBuilder.Entity<AkNotaDebitKreditBelian>()
                .HasOne(m => m.AkBelian)
                .WithMany(m => m.AkNotaDebitKreditBelian)
                .HasForeignKey(m => m.AkBelianId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            //AkNotaDebitKreditBelian End

            //AkPV
            modelBuilder.Entity<AkPV>()
                .HasOne(m => m.JKW)
                .WithMany(t => t.AkPV)
                .HasForeignKey(m => m.JKWId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            modelBuilder.Entity<AkPV2>()
                .HasOne(m => m.AkBelian)
                .WithMany(t => t.AkPV2)
                .HasForeignKey(m => m.AkBelianId)
                .OnDelete(DeleteBehavior.ClientNoAction);

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
                    .HasOne(m => m.JBank)
                    .WithMany(t => t.AkPV)
                    .HasForeignKey(m => m.JBankId)
                    .OnDelete(DeleteBehavior.NoAction);

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

            modelBuilder.Entity<AkPV>()
                    .HasOne(m => m.JBahagian)
                    .WithMany(t => t.AkPV)
                    .HasForeignKey(m => m.JBahagianId)
                    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AkPV>()
                .HasOne(m => m.JCaraBayar!)
                .WithMany(t => t.AkPV)
                .HasForeignKey(m => m.JCaraBayarId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<AkPVGanda>()
                .HasOne(m => m.JCaraBayar!)
                .WithMany(t => t.AkPVGanda)
                .HasForeignKey(m => m.JCaraBayarId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<AkPVGanda>()
                .HasOne(m => m.JBank!)
                .WithMany(t => t.AkPVGanda)
                .HasForeignKey(m => m.JBankId)
                .OnDelete(DeleteBehavior.NoAction);
            //AKPV end

            //Biz Channel (CIMB EFT)
            modelBuilder.Entity<AkCimbEFT1>()
                .HasOne(m => m.AkPembekal)
                .WithMany(t => t.AkCimbEFT1)
                .HasForeignKey(m => m.AkPembekalId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<AkCimbEFT1>()
                .HasOne(m => m.SuPekerja)
                .WithMany(t => t.AkCimbEFT1)
                .HasForeignKey(m => m.SuPekerjaId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<AkCimbEFT1>()
                .HasOne(m => m.SuAtlet)
                .WithMany(t => t.AkCimbEFT1)
                .HasForeignKey(m => m.SuAtletId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<AkCimbEFT1>()
                .HasOne(m => m.SuJurulatih)
                .WithMany(t => t.AkCimbEFT1)
                .HasForeignKey(m => m.SuJurulatihId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<AkCimbEFT1>()
                    .HasOne(m => m.JBank)
                    .WithMany(t => t.AkCimbEFT1)
                    .HasForeignKey(m => m.JBankId)
                    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AkCimbEFT>()
                    .HasOne(m => m.AkBank)
                    .WithMany(t => t.AkCimbEFT)
                    .HasForeignKey(m => m.AkBankId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired();
            //Biz Channel (CIMB EFT) end

            //Penyata Pemungut

            modelBuilder.Entity<AkPenyataPemungut>()
                .HasOne(m => m.SuPekerja)
                .WithMany(t => t.AkPenyataPemungut)
                .HasForeignKey(m => m.SuPekerjaId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<AkPenyataPemungut>()
                    .HasOne(m => m.AkBank)
                    .WithMany(t => t.AkPenyataPemungut)
                    .HasForeignKey(m => m.AkBankId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired();

            modelBuilder.Entity<AkPenyataPemungut>()
                .HasOne(m => m.JCaraBayar)
                .WithMany(t => t.AkPenyataPemungut)
                .HasForeignKey(m => m.JCaraBayarId)
                .OnDelete(DeleteBehavior.NoAction);
            //Penyata Pemungut end

            //SPPENDAHULUAN
            modelBuilder.Entity<SpPendahuluanPelbagai>()
                    .HasOne(m => m.SuPekerja!)
                    .WithMany(t => t.SpPendahuluanPelbagai)
                    .HasForeignKey(m => m.SuPekerjaId)
                    .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<SpPendahuluanPelbagai>()
                    .HasOne(m => m.JBahagian)
                    .WithMany(t => t.SpPendahuluanPelbagai)
                    .HasForeignKey(m => m.JBahagianId)
                    .OnDelete(DeleteBehavior.Restrict);
            //SPPENDAHULUAN END

            //AkBank
            modelBuilder.Entity<AkBank>()
                    .HasOne(m => m.JBahagian)
                    .WithMany(t => t.AkBank)
                    .HasForeignKey(m => m.JBahagianId)
                    .OnDelete(DeleteBehavior.Restrict);
            //AkBank end

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

            modelBuilder.Entity<AkTunaiRuncit>()
                    .HasOne(m => m.JBahagian)
                    .WithMany(t => t.AkTunaiRuncit)
                    .HasForeignKey(m => m.JBahagianId)
                    .OnDelete(DeleteBehavior.Restrict);

            //AkTunaiRuncit end
            //AkTunaiLejar
            modelBuilder.Entity<AkTunaiLejar>()
                    .HasOne(m => m.JBahagian)
                    .WithMany(t => t.AkTunaiLejar)
                    .HasForeignKey(m => m.JBahagianId)
                    .OnDelete(DeleteBehavior.Restrict);
            //AkTunaiLejar end

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

            //AkJurnal
            modelBuilder.Entity<AkJurnal>()
                    .HasOne(m => m.JBahagian)
                    .WithMany(t => t.AkJurnal)
                    .HasForeignKey(m => m.JBahagianId)
                    .OnDelete(DeleteBehavior.Restrict);
            //AkJurnal End

            // SUPROFIL
            modelBuilder.Entity<SuProfil>()
            .HasOne(m => m.JKW)
            .WithMany(t => t.SuProfil)
            .HasForeignKey(m => m.JKWId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

            modelBuilder.Entity<SuProfil>()
            .HasOne(m => m.AkCarta)
            .WithMany(t => t.SuProfil)
            .HasForeignKey(m => m.AkCartaId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

            modelBuilder.Entity<SuProfil>()
            .HasOne(m => m.JBahagian)
            .WithMany(t => t.SuProfil)
            .HasForeignKey(m => m.JBahagianId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

            modelBuilder.Entity<SuProfil1>()
            .HasOne(m => m.JCaraBayar)
            .WithMany(t => t.SuProfil1)
            .HasForeignKey(m => m.JCaraBayarId)
            .OnDelete(DeleteBehavior.Restrict);
            // SUPROFIL END

            //set default value
            modelBuilder.Entity<AkJurnal>().Property(b => b.Catatan1).HasDefaultValue("");
            modelBuilder.Entity<AkJurnal>().Property(b => b.Catatan2).HasDefaultValue("");
            modelBuilder.Entity<AkJurnal>().Property(b => b.Catatan3).HasDefaultValue("");
            modelBuilder.Entity<AkJurnal>().Property(b => b.Catatan4).HasDefaultValue("");

        }

    }
}
