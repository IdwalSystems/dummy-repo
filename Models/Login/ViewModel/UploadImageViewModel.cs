using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace MSNK.Models.Login.ViewModel
{
    public class UploadImageViewModel
    {
        [Display(Name = "Logo")]
        public IFormFile Gambar { get; set; }
    }
}
