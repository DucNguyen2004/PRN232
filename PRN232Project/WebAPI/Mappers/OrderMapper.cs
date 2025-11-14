using BusinessObjects;
using DTOs;

namespace Mappers
{
    public class OrderMapper
    {
        public static OrderResponseDto ToDTO(Order entity)
        {
            return new OrderResponseDto
            {
                Id = entity.Id,
                User = new UserOrderResponseDto
                {
                    Fullname = entity.User.Fullname,
                    Phone = entity.User.Phone,
                    Email = entity.User.Email
                },
                OrderDate = entity.OrderDate,
                ShippingAddress = entity.ShippingAddress,
                DiscountPrice = entity.DiscountPrice,
                Message = entity.Message,
                OrderStatus = entity.OrderStatus,
                OrderDetails = entity.OrderDetails.Select(od => OrderDetailMapper.ToDTO(od)).ToList()
            };
        }
        public static Order ToEntity(OrderRequestDto dto)
        {
            return new Order
            {
                OrderDate = dto.OrderDate,
                ShippingAddress = dto.ShippingAddress,
                Message = dto.Message,
                DiscountPrice = dto.DiscountPrice,
                OrderStatus = "PENDING",
                OrderDetails = dto.OrderDetails.Select(od => OrderDetailMapper.ToEntity(od)).ToList(),
                CancelReason = ""
            };
        }
    }
}
