using API.Dtos.Shopping;
using Core.Domain.RepositoryInterfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class PurchaseMemoryStore : IPurchaseMemoryStore
    {
        private readonly ConcurrentDictionary<long, ShoppingCartItemDto> _store = new();

        public void Save(long purchaseId, ShoppingCartItemDto item)
        {
            _store[purchaseId] = item;
        }

        public ShoppingCartItemDto? Get(long purchaseId)
        {
            _store.TryGetValue(purchaseId, out var item);
            return item;
        }

        public void Remove(long purchaseId)
        {
            _store.TryRemove(purchaseId, out _);
        }
    }
}
