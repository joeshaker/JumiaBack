using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumia_Api.Domain.Models
{
    public class VariantAttribute
    {
        [Key]
        public int VariantAttributeId { get; set; }

        [Required]
        [ForeignKey("ProductVariant")]
        public int VariantId { get; set; }

        [MaxLength(50)]
        public string? AttributeName { get; set; }

      
        [MaxLength(100)]
        public string? AttributeValue { get; set; }

        // Navigation property
        public ProductVariant ProductVariant { get; set; }
    }
}
