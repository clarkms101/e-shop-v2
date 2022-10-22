using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EShop.Entity.DataBase.Models
{
    /// <summary>
    /// 優惠券
    /// </summary>
    public class Coupon : BaseModel
    {
        [Required] 
        public int Id { get; set; }

        /// <summary>
        /// 優惠券標題
        /// </summary>
        [Required]
        [Column(TypeName = "varchar(50)")]
        public string Title { get; set; }

        /// <summary>
        /// 優惠券代碼
        /// </summary>
        [Required]
        [Column(TypeName = "varchar(50)")]
        public string CouponCode { get; set; }

        /// <summary>
        /// 打折百分比
        /// ex: 90 => 打9折
        /// ex: 65 => 打65折
        /// </summary>
        [Required]
        public int Percent { get; set; }

        /// <summary>
        /// 是否啟用
        /// </summary>
        [Required]
        public bool IsEnabled { get; set; }

        /// <summary>
        /// 到期時間
        /// </summary>
        public DateTime DueDateTime { get; set; }
    }
}