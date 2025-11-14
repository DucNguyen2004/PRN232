using AutoMapper;
using BusinessObjects;
using DTOs;
using Repositories;

namespace Services
{
    public class OrderService : IOrderService
    {

        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IProductOptionRepository _productOptionRepository;
        private readonly ICouponUsageRepository _couponUsageRepository;
        private readonly ICartService _cartService;
        private readonly IMapper _mapper;

        public OrderService(
            IOrderRepository orderRepository,
            IProductRepository productRepository,
            IProductOptionRepository productOptionRepository,
            ICouponUsageRepository couponUsageRepository,
            ICartService cartService,
            IMapper mapper)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _productOptionRepository = productOptionRepository;
            _couponUsageRepository = couponUsageRepository ?? throw new ArgumentNullException(nameof(couponUsageRepository));
            _cartService = cartService;
            _mapper = mapper;
        }

        public async Task<OrderResponseDto> GetOrderByIdAsync(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            return _mapper.Map<OrderResponseDto>(order);
        }

        public async Task<IEnumerable<OrderResponseDto>> GetAllOrdersAsync()
        {
            var orders = await _orderRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<OrderResponseDto>>(orders);
        }

        public async Task<IEnumerable<OrderResponseDto>> GetAllOrdersByUserIdAsync(int userId)
        {
            var orders = await _orderRepository.GetAllByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<OrderResponseDto>>(orders);
        }

        public async Task<Order> PlaceOrderAsync(OrderRequestDto dto, int userId)
        {
            if (dto.CouponId > 0)
            {
                await _couponUsageRepository.AddUsageAsync(dto.CouponId, userId);
            }

            var newOrder = _mapper.Map<Order>(dto);
            newOrder.UserId = userId;
            for (int i = 0; i < newOrder.OrderDetails.Count; i++)
            {
                var detailDto = dto.OrderDetails.ElementAt(i);
                var newDetail = newOrder.OrderDetails.ElementAt(i);
                newDetail.ProductOptions = await _productOptionRepository.GetByIdsAsync(detailDto.ProductOptionIds, detailDto.ProductId);
            }

            await _cartService.ClearCart(userId);

            return await _orderRepository.AddAsync(newOrder);
        }

        public async Task UpdateOrderAsync(int id, OrderRequestDto dto)
        {
            var updatedOrder = _mapper.Map<Order>(dto);
            for (int i = 0; i < updatedOrder.OrderDetails.Count; i++)
            {
                var detailDto = dto.OrderDetails.ElementAt(i);
                var newDetail = updatedOrder.OrderDetails.ElementAt(i);
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
