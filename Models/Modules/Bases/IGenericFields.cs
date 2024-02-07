using System;

namespace MSNK.Models.Modules.Bases
{
    public interface IGenericFields
    {
        //Soft Delete
        public int FlHapus { get; set; }
        public DateTime? TarHapus { get; set; }
        public string SebabHapus { get; set; }
        //Soft Delete end
    }
}
