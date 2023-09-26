using MSNK.Models.Operations;

namespace MSNK.Models.Modules.PrintModel.Reporting
{
    public class PengumuranPrintModel
    {
        public string Kod { get; set; }
        public string Nama { get; set; }
        public decimal JumlahTunggakan { get; set; }
        public decimal Tertunggak { get; set; }
        public KelasTunggakan KelasTunggakan { get; set; }
    }
}
