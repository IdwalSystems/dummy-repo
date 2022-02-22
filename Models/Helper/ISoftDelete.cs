using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Helper
{
    public interface ISoftDelete
    {
        //Soft Delete
        
        public bool FlHapus { get; set; }
        public DateTime? TarHapus { get; set; }
        //Soft Delete end

    }
}
