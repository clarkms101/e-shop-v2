using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace e_shop_api.DataBase.Models
{
    public class Order : BaseModel
    {
        [Required] public int Id { get; set; }

        /// <summary>
        /// 會員Id(非會員的訪客，填null即可)
        /// </summary>
        public int? UserId { get; set; }

        /// <summary>
        /// 已付款
        /// </summary>
        public bool IsPaid { get; set; }

        /// <summary>
        /// 付款方式
        /// </summary>
        [Column(TypeName = "varchar(50)")]
        public string PaymentMethod { get; set; }

        /// <summary>
        /// 實際付款時間
        /// </summary>
        public DateTime? PaidDateTime { get; set; }

        /// <summary>
        /// 總金額
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// 購買者姓名
        /// </summary>
        [Required]
        [Column(TypeName = "varchar(50)")]
        public string UserName { get; set; }

        /// <summary>
        /// 購買者地址
        /// </summary>
        [Required]
        [Column(TypeName = "varchar(250)")]
        public string Address { get; set; }

        /// <summary>
        /// 購買者E-mail
        /// </summary>
        [Required]
        [Column(TypeName = "varchar(50)")]
        public string Email { get; set; }

        /// <summary>
        /// 購買者電話
        /// </summary>
        [Column(TypeName = "varchar(25)")]
        public string Tel { get; set; }

        /// <summary>
        /// 註記
        /// </summary>
        [Column(TypeName = "varchar(250)")]
        public string Message { get; set; }
    }
}