using System.Threading.Tasks;


using Convey;
using Convey.Auth;

using Exchange.Shared.Extensions;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Exchange.Shared.Security
{
    public static class Extensions
    {
        public static async Task<string> AuthenticateUsingJwtAsync(this HttpContext context)
        {
            var authentication = await context.AuthenticateAsync(JwtBearerDefaults.AuthenticationScheme).WithoutCapturingContext();

            return authentication.Succeeded ? authentication?.Principal?.Identity?.Name ?? string.Empty : string.Empty;
        }

        public static IConveyBuilder AddAuth(this IConveyBuilder builder)
        {
            builder.Services.AddTransient<IUserDetailsProvider, UserDetailsProvider>();

            return builder
                .AddJwt();
        }

        public static IApplicationBuilder UseAuth(this IApplicationBuilder app) =>
            app.UseAuthentication()
                .UseAuthorization()
                .UseAccessTokenValidator();
    }
}