using MSNK.Models.Modules.ViewModel;
using System.Collections.Generic;

namespace MSNK.Controllers
{
    internal class AbBukuVotDetailGroupByBahagianVotViewModel
    {
        public string JBahagian { get; set; }
        public string Vot { get; set; }
        public List<AbBukuVotDetailViewModel> AbBukuVotDetailViewModel { get; set; }
    }
}