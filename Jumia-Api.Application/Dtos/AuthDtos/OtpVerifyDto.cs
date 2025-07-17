using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumia_Api.Application.Dtos.AuthDtos
{
    public class OtpVerifyDto
    {
        public string Email { get; set; }
        public string OtpCode { get; set; }
    }
}
