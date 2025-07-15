using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumia_Api.Application.Interfaces
{
    public interface ICampaignEmailService
    {
        Task ProcessPendingCampaignsAsync(CancellationToken cancellationToken = default);
    }
}
