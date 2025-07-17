using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumia_Api.Application.Dtos.AiDtos
{
    public class AiQuestionResponseDto
    {
        public string Question { get; set; }
        public int? ProductId { get; set; }
        public string Answer { get; set; }
    }

}
