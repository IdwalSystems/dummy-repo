using Microsoft.AspNetCore.Mvc.Rendering;
using MSNK.Models.Modules;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Login.ViewModel
{
    public class RegisterViewModel
    {
         [Required(ErrorMessage = "Emel Diperlukan.")]
        [EmailAddress]
        [Display(Name = "Emel")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Katalaluan Diperlukan.")]
        [StringLength(100, ErrorMessage = "{0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Katalaluan")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Pengesahan Katalaluan")]
        [Compare("Password", ErrorMessage = "Katalaluan dan pengesahan katalaluan tidak sama")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Nama Penuh")]
        public string Nama { get; set; }

        public IEnumerable<SelectListItem> RoleList { get; set; }
        [Display(Name = "Peranan")]
        public string RoleSelected { get; set; }
        [DisplayName("Anggota")]
        public int? SuPekerjaId { get; set; }
        public SuPekerja SuPekerja { get; set; }
    }
}
