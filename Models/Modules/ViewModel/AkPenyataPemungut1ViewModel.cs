namespace MSNK.Models.Modules.ViewModel
{
    public class AkPenyataPemungut1ViewModel
    {
        public int Id { get; set; }
        public int AkTerimaId { get; set; }
        public int Indek { get; set; }
        public int JBahagianId { get; set; }
        public JBahagian JBahagian { get; set; }
        public int AkCartaId { get; set; }
        public AkCarta AkCarta { get; set; }
        public decimal Amaun { get; set; }
    }
}
