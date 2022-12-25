using DMSTask.DAL.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DMSTask.DAL.Models
{
    public class OrderHeader
    {
        public int Id { get; set; }
        [ForeignKey(nameof(Customer))]
        public int CustomerId { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? RequestDate { get; set; }
        public DateTime? DueDate { get; set; }
        public OrderStatus Status { get; set; }
        public int? TaxCode { get; set; }
        public decimal TaxValue { get; set; }
        public int? DiscountCode { get; set; }
        public decimal DiscountValue { get; set; }
        public decimal TotalPrice { get; set; }
        public Customer Customer { get; set; }

        public virtual ICollection<OrderDetails> OrderDetails { get; set; }
    }
}
