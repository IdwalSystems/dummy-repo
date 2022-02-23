using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MSNK.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static MSNK.Models.Modules.AppLog;

namespace MSNK.Models.Modules
{
    public class AppLog
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime LgDate { get; set; }
        public string LgModule { get; set; }
        public string LgOperation { get; set; }
        public string LgNote { get; set; }
        public string NoRujukan { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Jumlah { get; set; }
        public string SysCode { get; set; }
        
    }
}
