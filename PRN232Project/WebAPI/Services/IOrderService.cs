using BusinessObjects;
using DTOs;

namespace Services
{
    public interface IOrderService
    {
        Task<OrderResponseDto> GetOrderByIdAsync(int id);
        Task<IEnumerable<OrderResponseDto>> GetAllOrdersAsync();
        Task<IEnumerable<OrderResponseDto>> GetAllOrdersByUserIdAsync(int userId);
        Task<Order> PlaceOrderAsync(OrderRequestDto dto, int userId);
        Task UpdateOrderAsync(int id, OrderRequestDto dto);
        Task UpdateOrderStatusAsync(int id, string newStatus);
    }
}
