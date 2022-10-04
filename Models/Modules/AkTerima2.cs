using MSNK.Models.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MSNK.Models.Modules
{
    public class AkTerima2 : IRecon
    {
        // note:
        // FlJenis = 0 || null ( Lain - lain )
        // FlJenis = 1 ( Cek Cawangan Ini )
        // FlJenis = 2 ( Cek Tempatan )
        // FlJenis = 3 ( Cek Luar )
        // FlJenis = 4 ( Cek Antarabangsa )
        // ..
        //field
        public int Id { get; set; }
        public int AkTerimaId { get; set; }
        public AkTerima AkTerima { get; set; }
        [DisplayName("Cara Bayar")]
        public int JCaraBayarId { get; set; }
        [DisplayName("Amaun RM")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amaun { get; set; }
        [MaxLength(10)]
        public string NoCek { get; set; } 
        public int JenisCek { get; set; } 
        [MaxLength(4)]
        public string KodBankCek { get; set; } 
        [MaxLength(100)]
        public string TempatCek { get; set; }
        [MaxLength(30)]
        public string NoSlip { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? TarSlip { get; set; }
        public int? AkPenyataPemungutId { get; set; }
        public ICollection<AkPadananPenyata> AkPadananPenyata { get; set; }
        //field end


        //Relationship
        public JCaraBayar JCaraBayar { get; set; }
        public ICollection<AkPenyataPemungut2> AkPenyataPemungut2 { get; set; }

        //relationship end

        public int FlTunai { get; set; }
        public DateTime? TarTunai { get; set; }


    }
}