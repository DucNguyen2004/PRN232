using BusinessObjects;
using DTOs;

namespace Mappers
{
    public class ProductOptionMapper
    {
        public static ProductOptionResponseDto ToDTO(ProductOption entity)
        {
            return new ProductOptionResponseDto
            {
                Id = entity.OptionValueId,
                Name = entity.OptionValue.Option.Name,
                Type = entity.OptionValue.Name,
                DeltaPrice = entity.DeltaPrice
            };
        }

        public static ProductOption ToEntity(ProductOptionRequestDto dto)
        {
            return new ProductOption
            {
                OptionValueId = dto.OptionValueId,
                DeltaPrice = dto.DeltaPrice
            };
        }
    }
}
