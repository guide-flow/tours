using Infrastructure.Database;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Tour.Authorization;

namespace Tour.Startup
{
    public static class AuthConfiguration
    {
        public static IServiceCollection ConfigureAuth(this IServiceCollection services, IConfiguration configuration)
        {
            var issuer = Environment.GetEnvironmentVariable("ISSUER")!;
            var audience = Environment.GetEnvironmentVariable("AUDIENCE")!;
            var signingKey = Environment.GetEnvironmentVariable("SECRET_KEY")!;

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = issuer,
                        ValidAudience = audience,
                        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(signingKey))
                    };
                });

            services.AddAuthentication("HeaderAuth")
                .AddScheme<AuthenticationSchemeOptions, HeaderAuthenticationHandler>("HeaderAuth", null);

            services.AddAuthorization(options =>
            {
                options.AddPolicy("administratorPolicy", policy => policy.RequireRole("Admin"));
                options.AddPolicy("touristPolicy", policy => policy.RequireRole("Tourist"));
                options.AddPolicy("authorPolicy", policy => policy.RequireRole("Author"));
            });
            return services;
        }

        public static void ApplyMigrations(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ToursContext>();
            dbContext.Database.Migrate();
        }
    }
}
