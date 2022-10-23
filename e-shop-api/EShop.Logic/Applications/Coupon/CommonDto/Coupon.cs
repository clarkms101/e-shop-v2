using System.ComponentModel.DataAnnotations;

namespace EShop.Logic.Applications.Coupon.CommonDto
{
    public class Coupon
    {
        public int CouponId { get; set; }
        
        [Required]
        [StringLength(50)]
        public string Title { get; set; }
        
        [Required]
        [StringLength(50)]
        public string CouponCode { get; set; }

        /// <summary>
        /// 90 => 9折
        /// </summary>
        [Required]
        public int Percent { get; set; }

        public bool IsEnabled { get; set; }
        
        public long DueDateTimeStamp { get; set; }
    }
}