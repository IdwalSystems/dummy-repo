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
        //
        // parameter : tarikh related 
        [Display(Name = "Julat")]
        public string tarikhDari { get; set; }
        public string tarikhHingga { get; set; }
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
