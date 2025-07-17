using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumia_Api.Application.Dtos.AuthDtos
{
    public class PersonalDetailsDto
    {
        [Required]
        public string FirstName { get; set; }
        
        [Required]
        public string LastName { get; set; }
        
        [Required , Phone]
        public string PhoneNumber { get; set; }
        
        [Required]
        public DateTime BirthDate { get; set; }

    }
}
