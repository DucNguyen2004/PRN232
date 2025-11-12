using BusinessObjects;
using DTOs;
using Mappers;
using Repositories;

namespace Services
{
    public class OrderService : IOrderService
    {

        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IProductOptionRepository _productOptionRepository;
        private readonly ICouponRepository _couponRepository;
        private readonly ICouponUsageRepository _couponUsageRepository;
        private readonly ICartService _cartService;

        public OrderService(
            IOrderRepository orderRepository,
            IProductRepository productRepository,
            IProductOptionRepository productOptionRepository,
            ICouponRepository couponRepository,
            ICouponUsageRepository couponUsageRepository,
            ICartService cartService)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _productOptionRepository = productOptionRepository;
            _couponRepository = couponRepository ?? throw new ArgumentNullException(nameof(couponRepository));
            _couponUsageRepository = couponUsageRepository ?? throw new ArgumentNullException(nameof(couponUsageRepository));
            _cartService = cartService;
        }

        public async Task<OrderResponseDto> GetOrderByIdAsync(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            return Mappers.OrderMapper.ToDTO(order);
        }

        public async Task<IEnumerable<OrderResponseDto>> GetAllOrdersAsync()
        {
            var orders = await _orderRepository.GetAllAsync();
            var responses = orders.Select(OrderMapper.ToDTO).ToList();

            return responses;
        }

        public async Task<IEnumerable<OrderResponseDto>> GetAllOrdersByUserIdAsync(int userId)
        {
            var orders = await _orderRepository.GetAllByUserIdAsync(userId);
            var responses = orders.Select(OrderMapper.ToDTO).ToList();

            return responses;
        }

        public async Task<Order> PlaceOrderAsync(OrderRequestDto dto, int userId)
        {
            // increase sold quantity for each product in the order
            //foreach (var detail in dto.OrderDetails)
            //{
            //    var product = await _productRepository.GetByIdAsync(detail.ProductId);
            //    if (product != null)
            //    {
            //        product.Sold += detail.Quantity;
            //        await _productRepository.UpdateAsync(product.Id, product);
            //    }
            //}

            // increase coupon usage count if applicable
            if (dto.CouponId > 0)
            {
                await _couponUsageRepository.AddUsageAsync(dto.CouponId, userId);
            }

            var newOrder = Mappers.OrderMapper.ToEntity(dto);
            for (int i = 0; i < newOrder.OrderDetails.Count; i++)
            {
                OrderDetailRequestDto detailDto = dto.OrderDetails.ElementAt(i);
                OrderDetail newDetail = newOrder.OrderDetails.ElementAt(i);
                newDetail.ProductOptions = await _productOptionRepository.GetByIdsAsync(detailDto.ProductOptionIds, detailDto.ProductId);
            }
            newOrder.UserId = userId;

            await _cartService.ClearCart(userId);

            return await _orderRepository.AddAsync(newOrder);
        }

        public async Task UpdateOrderAsync(int id, OrderRequestDto dto)
        {
            var updatedOrder = Mappers.OrderMapper.ToEntity(dto);
            for (int i = 0; i < updatedOrder.OrderDetails.Count; i++)
            {
                OrderDetailRequestDto detailDto = dto.OrderDetails.ElementAt(i);
                OrderDetail newDetail = updatedOrder.OrderDetails.ElementAt(i);
                newDetail.ProductOptions = await _productOptionRepository.GetByIdsAsync(detailDto.ProductOptionIds, detailDto.ProductId);
            }

            await _orderRepository.UpdateAsync(id, updatedOrder);
        }

        public async Task UpdateOrderStatusAsync(int id, string newStatus)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
            {
                throw new ArgumentException($"Order with ID {id} not found.");
            }

            order.OrderStatus = newStatus;
            await _orderRepository.UpdateAsync(id, order);
        }
    }
}
