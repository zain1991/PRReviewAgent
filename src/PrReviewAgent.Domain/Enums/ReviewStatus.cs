using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrReviewAgent.Domain.Entities
{
    public enum ReviewStatus
    {
        Pending,
        Processing,
        Completed,
        Failed
    }
}
