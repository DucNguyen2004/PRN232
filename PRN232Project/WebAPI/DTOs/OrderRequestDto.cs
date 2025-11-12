namespace DTOs
{
    public class OrderRequestDto
    {
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public string ShippingAddress { get; set; }
        public string Message { get; set; }
        public int CouponId { get; set; }
        public int DiscountPrice { get; set; }
        public List<OrderDetailRequestDto> OrderDetails { get; set; } = new List<OrderDetailRequestDto>();
    }
}
