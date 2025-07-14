using MSNK.Models.Helper;
using System;

namespace MSNK.Models.Modules
{
    public class JJantina : AppLogHelper, ISoftDelete
    {
        public int Id { get; set; }
        public string Perihal { get; set; }

        //soft delete
        public int FlHapus { get; set; }
        public DateTime? TarHapus { get; set; }
        public string SebabHapus { get; set; }
        //soft delete end
    }
}
