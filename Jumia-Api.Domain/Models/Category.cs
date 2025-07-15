using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumia_Api.Domain.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public string Description { get; set; }
        public string ImageSrc { get; set; } // URL or path to the category image

        public int? ParentCategoryId { get; set; }

        // Navigation properties
        public Category ParentCategory { get; set; }
        public ICollection<Category> SubCategories { get; set; }
    }
}
