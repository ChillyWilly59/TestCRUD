using System.ComponentModel.DataAnnotations;

namespace TestCRUD.Models.Domain
{
    public class Provider
    {
        [Key]
        public int Id { get; set; }

        [MaxLength]
        public string Name { get; set; }

        public virtual List<Order> Orders { get; set; }
    }
}