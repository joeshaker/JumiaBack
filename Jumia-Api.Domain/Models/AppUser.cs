using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumia_Api.Domain.Models
{
    public class AppUser:IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
   
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }

        public string Gender { get; set; }


        //public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


    }
}
