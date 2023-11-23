using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TestCRUD.Models.Domain
{
    public class OrderItem
    {
        [Key]
        public int Id { get; set; }

        public int OrderId { get; set; }
        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; }

        [MaxLength]
        public string Name { get; set; }

        public decimal Quantity { get; set; }

        [MaxLength]
        public string Unit { get; set; }
    }
}