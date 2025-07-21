using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace Jumia_Api.Application.Dtos.RatingDtos
{
    public class RatingUpdateDto
    {
        public int RatingId { get; set; } // Required to identify the record
        [Range(1, 5)]
        public int Stars { get; set; }
        public string Comment { get; set; }

    }
}
