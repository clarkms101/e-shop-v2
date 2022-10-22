using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using e_shop_api.Core.Enumeration;

namespace EShop.Entity.DataBase.Models
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
        
        /// <summary>
        /// 權限
        /// </summary>
        [Required]
        [Column(TypeName = "varchar(50)")]
        public Permission Permission { get; set; }
    }
}