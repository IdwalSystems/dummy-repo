using System;

namespace MSNK.Models.Helper
{
    public interface ISoftDelete
    {
        //Soft Delete
        
        public int FlHapus { get; set; }
        public DateTime? TarHapus { get; set; }
        public string SebabHapus { get; set; }
        //Soft Delete end

    }
}
