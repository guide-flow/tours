using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using static Google.Rpc.Context.AttributeContext.Types;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace Tour.Authorization
{
    public class HeaderAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public HeaderAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)
            : base(options, logger, encoder, clock) { }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var id = Request.Headers["X-User-Id"].FirstOrDefault();
            var email = Request.Headers["X-User-Email"].FirstOrDefault();
            var role = Request.Headers["X-User-Role"].FirstOrDefault();

            if (id == null || email == null || role == null)
            {
                return Task.FromResult(AuthenticateResult.Fail("Missing user headers"));
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, id),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, role)
            };

            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}
