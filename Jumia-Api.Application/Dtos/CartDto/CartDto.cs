namespace Jumia_Api.Application.Dtos.CartDto
{
    public class CartDto
    {
        public int CartId { get; set; }
        public int CustomerId { get; set; }
        public List<CartItemDto> CartItems { get; set; } = new();
        public decimal TotalPrice { get; set; }
        public int TotalQuantity { get; set; }
    }

}
