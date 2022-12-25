using DMSTask.DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSTask.BLL.ViewModel
{
    public class ItemVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please Enter Name ")]
        [Display(Name = "Item Name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please Enter Description ")]
        [Display(Name = "Description")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Please Choose UOM ")]
        [Display(Name = "UOM")]
        public int UOMId { get; set; }
        [Required(ErrorMessage = "Please Enter Quantity ")]
        [Display(Name = "Quantity")]
        public int Quantity { get; set; }
        [Display(Name = "Price")]
        [Required(ErrorMessage = "Please Enter Price ")]
        public decimal Price { get; set; }
    }
}
