using MSNK.Models.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MSNK.Models.Modules
{
    public class AkCarta : AppLogHelper, ISoftDelete
    {
        
        //field
        public int Id { get; set; }
        [MaxLength(6)]
        [Required(ErrorMessage = "Kod Carta Diperlukan.")]
        [RegularExpression(@"^(([A-Z])\d{5})*$", ErrorMessage = "Contoh A99999.")]
        public string Kod { get; set; }
        [MaxLength(100)]
        [Required(ErrorMessage = "Perihal Diperlukan.")]
        public string Perihal { get; set; }
        [MaxLength(1)]
        [Display(Name = "Debit / Kredit")]
        [Required(ErrorMessage = "Pilih Debit atau Kredit.")]
        public string DebitKredit { get; set; }
        [MaxLength(1)]
        [Display(Name = "Umum / Detail")]
        [Required(ErrorMessage = "Pilih Umum atau Detail.")]
        public string UmumDetail { get; set; }
        [MaxLength(100)]
        [Display(Name = "Catatan 1")]
        public string Catatan1 { get; set; }
        [MaxLength(100)]
        [Display(Name = "Catatan 2")]
        public string Catatan2 { get; set; }
        [Display(Name = "Baki RM")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Baki { get; set; }
        [Display(Name = "Dalam Peruntukan")]
        [DefaultValue(true)]
        public bool IsBajet { get; set; }
        //field end

        //Relationship
        [Display(Name = "KW")]
        [Required(ErrorMessage = "Kumpulan Wang Diperlukan.")]
        public int JKWId { get; set; }
        [Display(Name = "KW")]
        public JKW JKW { get; set; }
        [Display(Name = "Jenis")]
        [Required(ErrorMessage = "Jenis Diperlukan.")]
        public int JJenisId { get; set; }
        [Display(Name = "Jenis")]
        public JJenis JJenis { get; set; }
        [Display(Name = "Paras")]
        [Required(ErrorMessage = "Paras Diperlukan.")]
        public int JParasId { get; set; }
        [Display(Name = "Paras")]
        public JParas JParas { get; set; }
        public ICollection<AkTerima1> AkTerima1 { get; set; }
        public ICollection<AkBank> AkBank { get; set; }
        public virtual ICollection<AkAkaun> AkAkaun1 { get; set; }
        public virtual ICollection<AkAkaun> AkAkaun2 { get; set; }
        public ICollection<AkPO1> AkPO1 { get; set; }
        public ICollection<AkPOLaras1> AkPOLaras1 { get; set; }
        public ICollection<AkJurnal1> AkJurnal1 { get; set; }
        public ICollection<AkBelian> KodObjekAP { get; set; }
        public ICollection<AkBelian1> AkBelian1 { get; set; }
        public ICollection<AkPV1> AkPV1 { get; set; }
        public ICollection<AbBukuVot> Vot { get; set; }
        public ICollection<SpPendahuluanPelbagai> SpPendahuluanPelbagai { get; set; }
        public ICollection<AkTunaiRuncit> AkTunaiRuncit { get; set; }
        public ICollection<AkTunaiCV1> AkTunaiCV1 { get; set; }
        public ICollection<AkNotaMinta1> AkNotaMinta1 { get; set; }

        public ICollection<AbWaran1> AbWaran1 { get; set; }
        //relationship end

        //soft delete
        public int FlHapus { get; set; }
        public DateTime? TarHapus { get; set; }
        //soft delete end
    }
}
