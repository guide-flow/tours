using API.Dtos.Shopping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.RepositoryInterfaces
{
    public interface IPurchaseMemoryStore
    {
        void Save(long purchaseId, ShoppingCartItemDto item);
        ShoppingCartItemDto? Get(long purchaseId);
        void Remove(long purchaseId);
    }
}
