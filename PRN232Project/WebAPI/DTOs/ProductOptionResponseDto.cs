namespace DTOs
{
    public class ProductOptionResponseDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Type { get; set; }
        public int DeltaPrice { get; set; }
    }
}
