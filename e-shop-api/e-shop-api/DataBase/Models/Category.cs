using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace e_shop_api.DataBase.Models
{
    public class Category : BaseModel
    {
        [Required] public int Id { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        public string CategoryName { get; set; }
        
        [Column(TypeName = "varchar(150)")]
        public string Description { get; set; }
    }
}