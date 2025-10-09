using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Events
{
    public record PurchaseRejected(long PurchaseId, long UserId, long TourId, string Reason);
}
