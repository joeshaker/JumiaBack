using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumia_Api.Domain.Models
{
    public class ProductAttribute
    {
        [Key]
        public int AttributeId { get; set; }

        [Required]
        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(10)]
        [RegularExpression("text|number|boolean|select")]
        public string Type { get; set; }

        public string PossibleValues { get; set; } // JSON string

        public bool IsRequired { get; set; } = false;

        public bool IsFilterable { get; set; } = false;

        // Navigation properties
        public Category Category { get; set; }
        public ICollection<ProductAttributeValue> AttributeValues { get; set; }
    }
}
