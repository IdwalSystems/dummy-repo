using MSNK.Models.Administration;
using MSNK.Models.Modules.ViewModel;
using System.Collections.Generic;
using System.Linq;

namespace MSNK.Models.Modules.PrintModel.Reporting
{
    public class LPB001PrintModel : AbBukuVot
    {
        public string Username { get; set; }
        public IEnumerable<AbBukuVot> AbBukuVot { get; set; }
        public string KodLaporan { get; set; }
        public string ParamTajuk { get; set; }
        public string ParamCarta { get; set; }
        public string ParamKW { get; set; }
        public string ParamBahagian { get; set; }
        public string ParamTarikh { get; set; }
        public string Penyemak { get; set; }
        public string JawatanPenyemak { get;set; }
        public string Pelulus { get; set; }
        public string JawatanPelulus { get; set; }
        public CompanyDetails CompanyDetails { get; set; }
        public List<AbBukuVotDetailViewModel> abBukuVotDetailList { get; set; }
        public IEnumerable<IGrouping<object,AbBukuVotDetailViewModel>> GroupDataBukuVotList { get; set; }
    }
}
