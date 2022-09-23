using System.ComponentModel;

namespace MSNK.Models.Login.ViewModel
{
    public class ApplicationUserViewModel : EditSignViewModel
    {
        public string id { get; set; }
        public string Nama { get; set; }
        [DisplayName("Tandatangan")]
        public string Tandatangan { get; set; }
    }
}
