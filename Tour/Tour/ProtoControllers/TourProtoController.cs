using API.Dtos;
using API.ServiceInterfaces;
using Common.Enums;
using Grpc.Core;
using Proto = GrpcServiceTranscoding.Tours; 

namespace Tour.ProtoControllers
{
    public class TourProtoController : Proto.ToursService.ToursServiceBase
    {
        private readonly ITourService _tourService;
        private readonly ILogger<TourProtoController> _logger;

        private const string UserIdHeader = "X-User-Id";
        private const string RoleHeader = "X-User-Role";

        public TourProtoController(ITourService tourService, ILogger<TourProtoController> logger)
        {
            _tourService = tourService;
            _logger = logger;
        }

        private (string userId, string role) GetUserFromContext(ServerCallContext context)
        {
            var http = context.GetHttpContext();

            string? userId = null;
            string? role = null;

            if (http?.Request?.Headers.TryGetValue(UserIdHeader, out var uidVal) == true)
                userId = uidVal.ToString();
            if (http?.Request?.Headers.TryGetValue(RoleHeader, out var roleVal) == true)
                role = roleVal.ToString();

            if (string.IsNullOrWhiteSpace(userId))
                userId = context.RequestHeaders?.FirstOrDefault(h => h.Key.Equals(UserIdHeader, StringComparison.OrdinalIgnoreCase))?.Value;
            if (string.IsNullOrWhiteSpace(role))
                role = context.RequestHeaders?.FirstOrDefault(h => h.Key.Equals(RoleHeader, StringComparison.OrdinalIgnoreCase))?.Value;

            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(role))
                throw new RpcException(new Status(StatusCode.Unauthenticated, $"Missing {UserIdHeader}/{RoleHeader} headers or claims."));

            return (userId, role);
        }

        private static void EnsureRole(string role)
        {
            if (role != "Author")
                throw new RpcException(new Status(StatusCode.PermissionDenied, $"Role '{role}' is not allowed to create or update."));
        }

        private static Proto.TourDifficulty ToProtoDifficulty(Difficulty d)
            => (Proto.TourDifficulty)(int)d;

        private static Difficulty ToDomainDifficulty(Proto.TourDifficulty d)
            => (Difficulty)(int)d;

        private static Proto.Tag ToProtoTag(TagDto t) => new Proto.Tag { Id = t.Id, Name = t.Name ?? string.Empty };
        private static TagDto ToDtoTag(Proto.Tag t) => new TagDto { Id = t.Id, Name = t.Name };

        private static Proto.Tour MapToProto(TourDto tour)
        {
            var msg = new Proto.Tour
            {
                Id = tour.Id,
                Title = tour.Title ?? string.Empty,
                Description = tour.Description ?? string.Empty,
                Difficulty = ToProtoDifficulty(tour.Difficulty),        
                Price = (double)tour.Price,
                Status = (Proto.TourStatus)tour.Status,                    
                AuthorId = tour.AuthorId ?? string.Empty,
            };

            if (tour.Tags is IEnumerable<TagDto> dtos)
                msg.Tags.AddRange(dtos.Select(ToProtoTag));

            return msg;
        }


        public override async Task<Proto.Tour> CreateTour(Proto.CreateUpdateTour request, ServerCallContext context)
        {
            var (userId, role) = GetUserFromContext(context);
            EnsureRole(role);

            _logger.LogInformation("CreateTour by {UserId} ({Role})", userId, role);

            _logger.LogInformation($"Tags for tour {request.Tags.Select(ToDtoTag).ToList()}");

            var created = await _tourService.CreateTourAsync(new TourDto
            {
                Title = request.Title,
                Description = request.Description,
                Difficulty = ToDomainDifficulty(request.Difficulty),
                Tags = request.Tags.Select(ToDtoTag).ToList(),
            }, userId);

            return MapToProto(created);
        }

    }
}
