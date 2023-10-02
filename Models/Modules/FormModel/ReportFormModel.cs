using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules.FormModel
{
    public class ReportFormModel
    {
        // tajuk laporan
        public string kodLaporan { get; set; }
        //
        // parameter : input selection
        [Display(Name = "Kump Wang")]
        public int? JKWId { get; set; }
        public JKW JKW { get; set; }
        [Display(Name = "Bahagian")]
        public int? JBahagianId { get; set; }
        public JBahagian JBahagian { get; set; }
        public int? AkPembekalId { get; set; }
        public AkPembekal AkPembekal { get; set; }
        public int? JPenyemakId { get; set; }
        public JPenyemak JPenyemak { get; set; }

        public int? JPelulusId { get; set; }
        public JPelulus JPelulus { get; set; }
        //
        // parameter : input single
        public string Tahun { get; set; } = DateTime.Now.ToString("yyyy");
        //
        // parameter : input range (id)
        public int IdDari { get; set; }
        public int IdHingga { get; set; } 
        //
        
        // parameter : tarikh related 
        [Display(Name = "Julat")]
        public string tarikhDari { get; set; } = DateTime.Parse("01/01/" + DateTime.Now.ToString("yyyy")).ToString("yyyy-MM-dd");
        public string tarikhHingga { get; set; } = DateTime.Now.ToString("yyyy-MM-dd");
        [Display(Name = "Bulan / Tahun")]
        public DateTime bulanTahun { get; set; }
        //
        // parameter : flag related
        [Display(Name = "Status")]
        public int status { get; set; }
        [Display(Name = "Susunan")]
        public int susunan { get; set; }
        //
    }
}
