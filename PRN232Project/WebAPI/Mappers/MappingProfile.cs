using AutoMapper;
using BusinessObjects;
using DTOs;

namespace Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserResponseDto>()
                .ForMember(dest => dest.RoleIds, opt => opt.MapFrom(src => src.Roles != null ? src.Roles.Select(r => r.Id).ToList() : new List<int>()))
                .ReverseMap()
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.RoleIds.Select(id => new Role { Id = id }).ToList()))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.Now));

            CreateMap<UserRequestDto, User>()
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.RoleIds.Select(id => new Role { Id = id }).ToList()));
            CreateMap<Category, CategoryResponseDto>().ReverseMap();

            CreateMap<ProductOption, ProductOptionResponseDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.OptionValueId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.OptionValue.Option.Name))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.OptionValue.Name));
            CreateMap<ProductOptionRequestDto, ProductOption>();

            CreateMap<Product, ProductResponseDto>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category))
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.ProductImages.Select(i => i.Image)))
                .ForMember(dest => dest.Options, opt => opt.MapFrom(src => src.ProductOptions));
            CreateMap<ProductRequestDto, Product>()
                .ForMember(dest => dest.ProductImages, opt => opt.MapFrom(src => src.Images.Select(i => new ProductImage { Image = i })))
                .ForMember(dest => dest.ProductOptions, opt => opt.MapFrom(src => src.Options))
                .ForMember(dest => dest.Sold, opt => opt.MapFrom(_ => 0))
                .ForMember(dest => dest.PrevStatus, opt => opt.MapFrom(src => src.Status));

            CreateMap<CartItem, CartItemResponseDto>()
                .ForMember(dest => dest.Product, opt => opt.MapFrom(src => new CartItemProductDto
                {
                    Id = src.Product.Id,
                    Name = src.Product.Name,
                    Price = src.Product.Price,
                    Status = src.Product.Status,
                    Image = src.Product.ProductImages.FirstOrDefault() != null ? src.Product.ProductImages.First().Image : string.Empty,
                    CategoryId = src.Product.CategoryId
                }))
                .ForMember(dest => dest.ProductOptions, opt => opt.MapFrom(src => src.ProductOptions));
            CreateMap<CartItemRequestDto, CartItem>()
                .ForMember(dest => dest.ProductOptions, opt => opt.MapFrom(src =>
                    src.ProductOptionIds.Select(id => new ProductOption { Id = id }).ToList()));

            CreateMap<OrderDetail, OrderDetailResponseDto>()
                .ForMember(dest => dest.Product, opt => opt.MapFrom(src => new ProductOrderDetailResponseDto
                {
                    Id = src.Product.Id,
                    Name = src.Product.Name,
                    Price = src.Product.Price
                }))
                .ForMember(dest => dest.ProductOptions, opt => opt.MapFrom(src => src.ProductOptions));
            CreateMap<OrderDetailRequestDto, OrderDetail>()
                .ForMember(dest => dest.ProductOptions, opt => opt.MapFrom(src =>
                    src.ProductOptionIds.Select(id => new ProductOption { Id = id }).ToList()));

            CreateMap<Order, OrderResponseDto>()
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => new UserOrderResponseDto
                {
                    Fullname = src.User.Fullname,
                    Phone = src.User.Phone,
                    Email = src.User.Email
                }))
                .ForMember(dest => dest.OrderDetails, opt => opt.MapFrom(src => src.OrderDetails));
            CreateMap<OrderRequestDto, Order>()
                .ForMember(dest => dest.OrderStatus, opt => opt.MapFrom(_ => "PENDING"))
                .ForMember(dest => dest.CancelReason, opt => opt.MapFrom(_ => string.Empty))
                .ForMember(dest => dest.OrderDetails, opt => opt.MapFrom(src => src.OrderDetails));

            CreateMap(typeof(PaginationResponseDto<>), typeof(PaginationResponseDto<>));
        }
    }
}