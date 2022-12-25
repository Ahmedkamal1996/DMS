using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSTask.DAL.Models
{
    [Table("UnitOfMeasure")]
    public class UnitOfMeasure
    {
        [Key]
        public int Id { get; set; }
        public string UOM { get; set; }
        public string Description { get; set; }
    }
}
