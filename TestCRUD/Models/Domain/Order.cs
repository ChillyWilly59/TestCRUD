using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TestCRUD.Models.Domain
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [MaxLength]
        public string Number { get; set; } = string.Empty;
        public DateTime Date { get; set; }

        public int ProviderId { get; set; }
        [ForeignKey("ProviderId")]
        public virtual Provider Provider { get; set; }

        public virtual List<OrderItem> OrderItems { get; set; }
        
    }
    

}
