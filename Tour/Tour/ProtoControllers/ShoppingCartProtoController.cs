using API.ServiceInterfaces;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Tour.Protos;

using ItemCreationDto = API.Dtos.Shopping.ShoppingCartItemCreationDto;

namespace Tour.ProtoControllers;

public class ShoppingCartProtoController : ShoppingCartService.ShoppingCartServiceBase
{
    private readonly IShoppingCartService cartService;

    public ShoppingCartProtoController(IShoppingCartService cartService)
    {
        this.cartService = cartService;
    }

    public override async Task<ShoppingCartItem> AddToCart(ItemCreation request, ServerCallContext context)
    {
        var result = await cartService.AddToCartAsync(new ItemCreationDto(request.TourId, request.TourName, (decimal)request.Price), 1);
        if (result.IsFailed)
            throw new RpcException(new Status(StatusCode.InvalidArgument, string.Join("; ", result.Errors)));

        var item = result.Value;
        var proto = new ShoppingCartItem
        {
            Id = item.Id,
            ShoppingCartId = item.ShoppingCartId,
            TourId = item.TourId,
            TourName = item.TourName,
            Price = (double)item.Price,
        };

        return proto;
    }

    public override async Task<Empty> RemoveFromCart(ShoppingCartItemId request, ServerCallContext context)
    {
        var result = await cartService.RemoveFromCartAsync(request.TourId, 1);
        if (result.IsFailed)
            throw new RpcException(new Status(StatusCode.InvalidArgument, string.Join("; ", result.Errors)));

        return new Empty();
    }
}
