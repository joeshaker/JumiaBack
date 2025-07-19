using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumia_Api.Application.Dtos.WishlistDtos
{
    public class WishlistDto
    {
        public int wishlistId { get; set; }
        public int customerId { get; set; }

        public List<WishlistItemDto> WishlistItems { get; set; } = new();

        public int totalQuantity { get; set; }
    }
}
