using TestCRUD.Models.Domain;

namespace TestCRUD.Models
{
    public class UpdateOrderViewModel
    {
        public int Id { get; set; }

        public string Number { get; set; } = string.Empty;
        public DateTime Date { get; set; }

        public int ProviderId { get; set; }
       
        public virtual Provider Provider { get; set; }

        public virtual List<OrderItem> OrderItems { get; set; }
    }
}
