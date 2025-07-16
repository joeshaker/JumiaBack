using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumia_Api.Application.Dtos.CategoryDtos
{
    public class UpdateCategoryDto
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? ImageSrc { get; set; }
        public int? ParentCategoryId { get; set; }
    }
}
