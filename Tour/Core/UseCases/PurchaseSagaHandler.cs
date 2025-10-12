using API.Dtos.Shopping;
using API.Events;
using API.ServiceInterfaces;
using Core.Domain.RepositoryInterfaces;
using Core.Domain.Shopping;
using Microsoft.Extensions.DependencyInjection;
using NATS.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Core.UseCases
{
    public class PurchaseSagaHandler
    {
        private readonly IConnection nats;
        private readonly IServiceScopeFactory scopeFactory;
        private readonly IPurchaseMemoryStore memoryStore;

        public PurchaseSagaHandler(IConnection nats, IServiceScopeFactory scopeFactory, IPurchaseMemoryStore memoryStore)
        {
            this.nats = nats;
            this.scopeFactory = scopeFactory;
            this.memoryStore = memoryStore;
        }

        public void Subscribe()
        {
            // Slušanje eventova iz IdentityService-a
            nats.SubscribeAsync("identity.user.*", async (s, e) =>
            {
                var msgType = e.Message.Subject.Split('.').Last();
                var json = Encoding.UTF8.GetString(e.Message.Data);

                switch (msgType)
                {
                    case "active":
                        var active = JsonSerializer.Deserialize<UserActive>(json);
                        if (active != null) await HandleUserActive(active);
                        break;
                    case "blocked":
                        var blocked = JsonSerializer.Deserialize<UserBlocked>(json);
                        if (blocked != null) await HandleUserBlocked(blocked);
                        break;
                }
            });

            nats.SubscribeAsync("followers.user.*", async (s, e) =>
            {
                var msgType = e.Message.Subject.Split('.').Last();
                var json = Encoding.UTF8.GetString(e.Message.Data);

                switch (msgType)
                {
                    case "follows":
                        var follows = JsonSerializer.Deserialize<UserFollowsAuthor>(json);
                        if (follows != null) await HandleUserFollows(follows);
                        break;
                    case "notfollows":
                        var notFollows = JsonSerializer.Deserialize<UserDoesNotFollowAuthor>(json);
                        if (notFollows != null) await HandleUserDoesNotFollow(notFollows);
                        break;
                }
            });
        }

        private async Task HandleUserActive(UserActive ua) {
            using var scope = scopeFactory.CreateScope();
            var purchaseRepository = scope.ServiceProvider.GetRequiredService<IPurchaseRepository>();
            var token = await purchaseRepository.GetByIdAsync(ua.PurchaseId);
            if (token == null) return;

            token.IdentityValidated = true;

            if (token.IdentityValidated && token.FollowerValidated)
            {
                token.Status = "Completed";

                PublishCompleted(token, 0);
            }
            await purchaseRepository.SaveChangesAsync();
        }

        private async Task HandleUserBlocked(UserBlocked ub) {
            using var scope = scopeFactory.CreateScope();
            var purchaseRepository = scope.ServiceProvider.GetRequiredService<IPurchaseRepository>();
            var token = await purchaseRepository.GetByIdAsync(ub.PurchaseId);
            if (token == null) return;
            token.IdentityValidated = false;
            token.Status = "Rejected";
            token.RejectReason = "User identity is blocked.";
            await purchaseRepository.SaveChangesAsync();

            await PublishRejected(token, "User is blocked");
        }

        private async Task HandleUserFollows(UserFollowsAuthor e)
        {
            using var scope = scopeFactory.CreateScope();
            var purchaseRepository = scope.ServiceProvider.GetRequiredService<IPurchaseRepository>();
            var token = await purchaseRepository.GetByIdAsync(e.PurchaseId);
            if (token == null) return;

            token.FollowerValidated = true;

            if (token.IdentityValidated && token.FollowerValidated)
            {
                token.Status = "Completed";

                PublishCompleted(token, e.AuthorId);
            }
            await purchaseRepository.SaveChangesAsync();
        }

        private async Task HandleUserDoesNotFollow(UserDoesNotFollowAuthor e)
        {
            using var scope = scopeFactory.CreateScope();
            var purchaseRepository = scope.ServiceProvider.GetRequiredService<IPurchaseRepository>();
            var token = await purchaseRepository.GetByIdAsync(e.PurchaseId);
            if (token == null) return;

            token.FollowerValidated = false;
            token.Status = "Rejected";
            token.RejectReason = "User does not follow author";

            await purchaseRepository.SaveChangesAsync();

            await PublishRejected(token, "User does not follow author");
        }

        private void PublishCompleted(TourPurchaseToken token, long authorId)
        {
            var evt = new PurchaseCompleted(token.Id, token.UserId, token.TourId, authorId);
            var payload = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(evt));
            nats.Publish("tours.purchase.completed", payload);
        }

        private async Task PublishRejected(TourPurchaseToken token, string reason)
        {
            using var scope = scopeFactory.CreateScope();
            var cartService = scope.ServiceProvider.GetRequiredService<IShoppingCartService>();

            var item = memoryStore.Get(token.Id);
            if (item != null)
            {
                await cartService.AddToCartAsync(
                     new ShoppingCartItemCreationDto(item.TourId,item.TourName,item.Price),
                     token.UserId);
                memoryStore.Remove(token.Id);
            }

            var evt = new PurchaseRejected(token.Id, token.UserId, token.TourId, reason);
            var payload = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(evt));
            nats.Publish("tours.purchase.rejected", payload);
        }
    }
}
