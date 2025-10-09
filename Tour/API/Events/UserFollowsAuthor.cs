using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Events
{
    public record UserFollowsAuthor(long PurchaseId, long UserId, long AuthorId);
}
