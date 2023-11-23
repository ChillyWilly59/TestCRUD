using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TestCRUD.Models.Domain;

namespace TestCRUD.Models
{
    public class AddOrderViewModel
    {
        [MaxLength]
        public string Number { get; set; }

        public DateTime Date { get; set; }
        public int ProviderId { get; set; }
        [ForeignKey("ProviderId")]
        public virtual Provider Provider { get; set; }

        public virtual List<OrderItem> OrderItems { get; set; }
    }
}
