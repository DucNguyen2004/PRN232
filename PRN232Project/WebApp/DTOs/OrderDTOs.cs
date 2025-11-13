namespace DTOs
{
    public class OrderDetailRequestDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public List<int> ProductOptionIds { get; set; } = new List<int>();
        public int Price { get; set; }
    }

    public class OrderDetailResponseDto
    {
        public int Id { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
        public ProductOrderDetailResponseDto Product { get; set; }
        public List<ProductOptionResponseDto> ProductOptions { get; set; } = new List<ProductOptionResponseDto>();
    }

    public class ProductOrderDetailResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
    }

    public class OrderRequestDto
    {
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public string ShippingAddress { get; set; }
        public string Message { get; set; }
        public int CouponId { get; set; }
        public int DiscountPrice { get; set; }
        public List<OrderDetailRequestDto> OrderDetails { get; set; } = new List<OrderDetailRequestDto>();
    }
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
