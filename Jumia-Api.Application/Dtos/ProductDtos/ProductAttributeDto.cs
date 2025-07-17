namespace Jumia_Api.Application.Dtos.ProductDtos
{
    public class ProductAttributeDto
    {
        public int AttributeId { get; set; }
        public string AttributeName { get; set; } = "";
        public List<string> Values { get; set; } = new();
    }

}
