using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jumia_Api.Domain.Models;

namespace Jumia_Api.Domain.Interfaces.Repositories
{
    public interface IRatingRepo:IGenericRepo<Rating>
    {
        Task<IEnumerable<Rating>> GetAllByProductId(int productId);
    }
}
