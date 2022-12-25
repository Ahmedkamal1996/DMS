using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSTask.DAL.Models
{
    public class OrderDetails
    {
        public int Id { get; set; }
        [ForeignKey(nameof(OrderHeader))]
        public int OrderId { get; set; }
        [ForeignKey(nameof(Items))]
        public int ItemId { get; set; }
        public decimal ItemPrice { get; set; }
        public int Qty { get; set; }
        public decimal TotalPrice { get; set; }
        [ForeignKey(nameof(UnitOfMeasure))]
        public int UOMId { get; set; }
        public decimal Tax { get; set; }
        public decimal Discount { get; set; }
        public virtual OrderHeader OrderHeader { get; set; }
        public virtual Items Items { get; set; } 
        public virtual UnitOfMeasure UnitOfMeasure { get; set; }
    }
}
