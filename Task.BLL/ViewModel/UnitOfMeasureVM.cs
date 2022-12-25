using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DMSTask.BLL.ViewModel
{
    public class UnitOfMeasureVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please Enter UOM ")]
        [Display(Name = "UOM")]
        public string UOM { get; set; }
        [Required(ErrorMessage = "Please Enter Description ")]
        [Display(Name = "Description")]
        public string Description { get; set; }
    }
}
