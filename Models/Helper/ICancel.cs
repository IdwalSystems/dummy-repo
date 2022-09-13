using System;

namespace MSNK.Models.Helper
{
    public interface ICancel
    {
        //Soft Delete

        public int FlBatal { get; set; }
        public DateTime? TarBatal { get; set; }
        //Soft Delete end
    }
}
