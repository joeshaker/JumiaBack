using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumia_Api.Application.Dtos.RatingDtos
{
    public class RatingCreateDto
    {
        public int CustomerId { get; set; }
        public int ProductId { get; set; }
        [Range(1, 5)]
        public int Stars { get; set; }
        public string Comment { get; set; }
    }
}
