namespace DTOs
{
    public class OrderResponseDto
    {
        public int Id { get; set; }
        public UserOrderResponseDto User { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public string ShippingAddress { get; set; }
        public string CouponCode { get; set; }
        public int DiscountPrice { get; set; }
        public string Message { get; set; }
        public string OrderStatus { get; set; }
        public List<OrderDetailResponseDto> OrderDetails { get; set; } = new List<OrderDetailResponseDto>();
    }

    public class UserOrderResponseDto
    {
        public string Fullname { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }
}
