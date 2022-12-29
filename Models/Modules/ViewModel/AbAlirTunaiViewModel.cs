namespace MSNK.Models.Modules.ViewModel
{
    public class AbAlirTunaiViewModel
    {
        public string NoAkaun { get; set; }
        public string NamaAkaun { get; set; }
        public string Bulan { get; set; }
        public decimal Jan { get; set; }
        public decimal Feb { get; set; }
        public decimal Mac { get; set; }
        public decimal Apr { get; set; }
        public decimal Mei { get; set; }
        public decimal Jun { get; set; }
        public decimal Jul { get; set; }
        public decimal Ogo { get; set; }
        public decimal Sep { get; set; }
        public decimal Okt { get; set; }
        public decimal Nov { get; set; }
        public decimal Dis { get; set; }
        public decimal Jan2 { get; set; }
        public decimal JumAkaun { get; set; }
        // nota:
        // KeluarMasuk: 0 = Baki Awal
        // KeluarMasuk: 1 = Masuk
        // KeluarMasuk: 2 = Keluar
        // KeluarMasuk: 3 = Baki Akhir
        public int KeluarMasuk { get; set; }
    }
}
