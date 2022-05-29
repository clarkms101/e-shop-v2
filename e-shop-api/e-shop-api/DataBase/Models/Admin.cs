using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace e_shop_api.DataBase.Models
{
    public class Admin : BaseModel
    {
        [Required] public int Id { get; set; }

        /// <summary>
        /// 後台帳號
        /// </summary>
        [Required]
        [Column(TypeName = "varchar(50)")]
        public string Account { get; set; }


        /// <summary>
        /// 後台帳號密碼
        /// </summary>
        [Required]
        [Column(TypeName = "varchar(50)")]
        public string Password { get; set; }
    }
}