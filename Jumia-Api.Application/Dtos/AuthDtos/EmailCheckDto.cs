using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumia_Api.Application.Dtos.AuthDtos
{
    public class EmailCheckDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
