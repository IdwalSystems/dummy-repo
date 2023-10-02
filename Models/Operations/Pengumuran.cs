using MSNK.Models.Modules;
using System;
using System.Collections.Generic;

namespace MSNK.Models.Operations
{
    public class Pengumuran
    {
        public string NoInvois { get; set; }
        public DateTime TarikhTerima { get; set; }
        public List<AkPV> AkPV { get; set; }
        public decimal Tunggak30 { get; set; }
        public decimal Tunggak60 { get; set; }
        public decimal Tunggak90 { get; set; }
        public decimal Tunggak180 { get; set; }
        public decimal Tunggak365 { get; set; }
        public decimal TunggakLebih365 { get; set; }
        public decimal JumlahTunggakan { get; set; }
    }
}
