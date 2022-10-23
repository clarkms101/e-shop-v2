using System.ComponentModel.DataAnnotations;

namespace EShop.Logic.Applications.Order.CommonDto
{
    public class OrderForm
    {
        [Required]
        [StringLength(50)]
        public string UserName { get; set; }
        
        [Required]
        [StringLength(250)]
        public string Address { get; set; }
        
        [Required]
        [StringLength(50)]
        public string Email { get; set; }
        
        [Required]
        [StringLength(50)]
        public string Tel { get; set; }
        
        [StringLength(250)]
        public string? Message { get; set; }
        
        [StringLength(50)]
        public string? PaymentMethod { get; set; }
    }
}