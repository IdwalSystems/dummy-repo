using System;

namespace MSNK.Models.Modules.ViewModel
{
    public class AkCimbEFT1ViewModel
    {
        public int Id { get; set; }
        public int Indek { get; set; }
        public int AkPVId { get; set; }
        public int FlPenerimaEFT { get; set; }
        public int? AkPembekalId { get; set; }
        public int? SuPekerjaId { get; set; }
        public int? SuAtletId { get; set; }
        public int? SuJurulatihId { get; set; }
        public string NoPV { get; set; }
        public string NoKP { get; set; }
        public string NoAkaun { get; set; }
        public string Penerima { get; set; }
        public string NoCekAtauEFT { get;set; }
        public DateTime Tarikh { get; set; }
        public string KodBank { get; set; }
        public decimal Amaun { get; set; }
        public int? FlStatus { get; set; }
    }
}
