namespace Jumia_Api.Application.Dtos.ProductDtos
{
    public class ProductVariantDto
    {
        public int VariantId { get; set; }
        public string VariantName { get; set; } = "";
        public decimal Price { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public int StockQuantity { get; set; }
        public string SKU { get; set; } = "";
        public string VariantImageUrl { get; set; } = "";
        public bool IsDefault { get; set; }
        public bool IsAvailable { get; set; }
        public List<VariantAttributeDto> Attributes { get; set; } = new();
    }

}
