using System;
using System.Collections.Generic;

namespace techXpress.DataAccess.Entities
{
    public class Coupon
    {
        public int CouponId { get; set; }
        public string Code { get; set; }
        public decimal Discount { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int UsedCount { get; set; }
    }
}