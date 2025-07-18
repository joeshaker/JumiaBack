using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumia_Api.Domain.Models
{
    public class CartItem
    {
        [Key]
        public int CartItemId { get; set; }

        [Required]
        [ForeignKey("Cart")]
        public int CartId { get; set; }

        [Required]
        [ForeignKey("Product")]
        public int ProductId { get; set; }

        [ForeignKey("ProductVariant")]
        public int? VariationId { get; set; }  

        [Required]
        public int Quantity { get; set; } = 1;

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal PriceAtAddition { get; set; }  // to lock price


        // Navigation properties
        public Cart Cart { get; set; }
        public Product Product { get; set; }
        public ProductVariant ProductVariant { get; set; } // Optional, if variations are used
    }
}
