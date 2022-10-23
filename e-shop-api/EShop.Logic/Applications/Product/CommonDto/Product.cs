using System.ComponentModel.DataAnnotations;

namespace EShop.Logic.Applications.Product.CommonDto
{
    public class Product
    {
        public int ProductId { get; set; }
        
        [Required]
        public int CategoryId { get; set; }
        
        [StringLength(50)]
        public string? Category { get; set; }
        
        [StringLength(250)]
        public string? Content { get; set; }
        
        [StringLength(250)]
        public string? Description { get; set; }
        
        [Required]
        [StringLength(250)]
        public string ImageUrl { get; set; }
        
        public bool IsEnabled { get; set; }
        
        public decimal OriginPrice { get; set; }
        
        public decimal Price { get; set; }
        
        [Required] 
        [StringLength(50)]
        public string Title { get; set; }
        
        [Required] 
        [StringLength(50)]
        public string Unit { get; set; }
        
        public int Num { get; set; }
    }
}