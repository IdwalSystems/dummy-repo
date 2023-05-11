using System;

namespace MSNK.Models.Modules.ViewModel
{
    public class AkAkaunGroupViewModel
    {
        public DateTime Tarikh { get; set; }
        public string SearchObjek { get; set; }
        public string Objek { get; set; }
        public string NoRujukan { get; set; }
        public decimal Debit { get; set; }
        public decimal Kredit { get; set; }
        public decimal Baki { get; set; }
    }
}
