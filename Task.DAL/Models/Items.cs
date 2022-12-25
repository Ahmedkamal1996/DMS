using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSTask.DAL.Models
{
    [Table("Items")]
    public class Items
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }    
        public string Description { get; set; }
        [ForeignKey(nameof(UnitOfMeasure))]
        public int UOMId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        public virtual UnitOfMeasure UnitOfMeasure { get; set; }
    }
}
