using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumia_Api.Domain.Models
{
    public class ProductAttributeValue
    {
        [Key]
        public int ValueId { get; set; }

        [Required]
        [ForeignKey("Product")]
        public int ProductId { get; set; }

        [Required]
        [ForeignKey("ProductAttribute")]
        public int AttributeId { get; set; }

        [Required]
        public string Value { get; set; }

        // Navigation properties
        public Product Product { get; set; }
        public ProductAttribute ProductAttribute { get; set; }
    }
}
