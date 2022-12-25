using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSTask.DAL.Models
{
    [Table("Customer")]
    public class Customer
    {
        [Key]
        public int CustomerCode { get; set; }
     
        public string CustomerDescriptionEn { get; set; }
       
        public string CustomerDescriptionAr { get; set; }

        [ForeignKey(nameof(IdentityUser))]
       
        public string UserId { get; set; }

        public virtual IdentityUser User { get; set; }
    }
}
