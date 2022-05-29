using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace e_shop_api.DataBase.Models
{
    public class Product
    {
        [Required] public int Id { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        public string Title { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        public string Category { get; set; }

        [Required]
        public decimal OriginPrice { get; set; }
        
        [Required]
        public decimal Price { get; set; }
        
        [Required]
        [Column(TypeName = "varchar(50)")]
        public string Unit { get; set; }
        
        [Column(TypeName = "varchar(250)")]
        public string ImageUrl { get; set; }
        
        [Column(TypeName = "varchar(250)")]
        public string Description { get; set; }
        
        [Column(TypeName = "varchar(250)")]
        public string Content { get; set; }
        
        public bool IsEnabled { get; set; }
        
        public int Num { get; set; }
    }
}