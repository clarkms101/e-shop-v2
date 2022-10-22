using System.ComponentModel.DataAnnotations;

namespace EShop.Entity.DataBase.Models
{
    public class OrderDetail : BaseModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int OrderId { get; set; }
        [Required]
        public int ProductId { get; set; }
        [Required]
        public int Qty { get; set; }
    }
}