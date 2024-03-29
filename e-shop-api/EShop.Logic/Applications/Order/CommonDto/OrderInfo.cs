﻿namespace EShop.Logic.Applications.Order.CommonDto
{
    public class OrderInfo
    {
        public int OrderId { get; set; }
        public string SerialNumber { get; set; }
        public int? UserId { get; set; }
        public bool IsPaid { get; set; }
        public string OrderStatus { get; set; }
        public string PaymentMethod { get; set; }
        public long CreateDateTime { get; set; }
        public long? PaidDateTime { get; set; }
        public decimal OriginTotalAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public string UserName { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Tel { get; set; }
        public string Message { get; set; }

        public List<OrderDetailInfo> OrderDetailInfos { get; set; }
    }
}