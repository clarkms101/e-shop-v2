using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EShop.Entity.DataBase.Models
{
    public class Category : BaseModel
    {
        [Required] public int Id { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        public string CategoryName { get; set; }
        
        [Column(TypeName = "varchar(150)")]
        public string? Description { get; set; }
    }
}