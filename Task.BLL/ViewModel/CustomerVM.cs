using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DMSTask.BLL.ViewModel
{
    public class CustomerVM
    {
        public int CustomerCode { get; set; }
        [Required(ErrorMessage = "Please Enter CustomerDescriptionEn ")]
        [Display(Name = "CustomerDescriptionEn")]
        public string CustomerDescriptionEn { get; set; }
        [Required(ErrorMessage = "Please Enter CustomerDescriptionAr ")]
        [Display(Name = "CustomerDescriptionAr")]
        public string CustomerDescriptionAr { get; set; }

        [ForeignKey(nameof(IdentityUser))]
        [Display(Name = "User")]
        [Required(ErrorMessage = "Please Choose User ")]
        public string UserId { get; set; }
    }
}
