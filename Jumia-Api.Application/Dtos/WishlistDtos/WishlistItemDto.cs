using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumia_Api.Application.Dtos.WishlistDtos
{
    public class WishlistItemDto
    {

        public int WishlistItemId { get; set; }

        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string MainImageUrl { get; set; }


        public decimal UnitPrice { get; set; } // price for product or variant
        public decimal DiscountPercentage { get; set; }
        public decimal FinalPrice => UnitPrice - (UnitPrice * DiscountPercentage / 100);

    }
}
