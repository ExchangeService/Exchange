using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using Exchange.Shared.Communication.Contexts;
using Exchange.Shared.Exceptions;

using JetBrains.Annotations;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

namespace Exchange.Shared.Security
{
    internal sealed class UserDetailsProvider : IUserDetailsProvider
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        private readonly IRequestContextAccessor requestContextAccessor;

        public UserDetailsProvider(IHttpContextAccessor contextAccessor, IRequestContextAccessor requestContextAccessor)
        {
            this.httpContextAccessor = contextAccessor;
            this.requestContextAccessor = requestContextAccessor;
        }

        public async Task<ClaimsPrincipal?> GetAsync()
        {
            var context = this.httpContextAccessor.HttpContext;
            if (context is null)
            {
                return null;
            }

            var authResult = await context.AuthenticateAsync(JwtBearerDefaults.AuthenticationScheme);
            return authResult?.Principal ?? null;
        }

        public async Task<string?> GetClaimValueAsync(string claim)
        {
            var user = await this.GetClaims();

            return user?.FirstOrDefault(e => string.Equals(e.Type, claim, StringComparison.OrdinalIgnoreCase))?.Value;
        }

        public Task<string?> GetEmailAsync() => this.GetClaimValueAsync(ClaimTypes.Email);

        public Task<string?> GetFirstNameAsync() => this.GetClaimValueAsync(ClaimTypes.GivenName);

        public Task<string?> GetPhoneNumberAsync() => this.GetClaimValueAsync(ClaimTypes.MobilePhone);

        public async Task<string> GetRequiredUserNameAsync() =>
            await this.GetUserNameAsync() ?? throw new CustomerUnauthenticatedException();

        public Task<string?> GetRoleAsync() => this.GetClaimValueAsync(ClaimTypes.Role);

        public Task<string?> GetSecondNameAsync() => this.GetClaimValueAsync(ClaimTypes.Surname);

        public Task<string?> GetTokenAsync() => this.httpContextAccessor.HttpContext?.GetTokenAsync("access_token") ?? Task.FromResult((string?)null);

        public Task<string?> GetUserNameAsync() => this.GetClaimValueAsync(ClaimTypes.Name);

        public async Task RelayAuthorizationAsync(Dictionary<string, string> headers)
        {
            var token = await this.GetTokenAsync();
            headers.Add(HeaderNames.Authorization, $"Bearer {token}");
        }

        [ItemCanBeNull]
        private async Task<List<Claim>> GetClaims()
        {
            var context = this.httpContextAccessor.HttpContext;
            if (context is { })
            {
                var authResult = await context.AuthenticateAsync(JwtBearerDefaults.AuthenticationScheme);
                return authResult?.Principal?.Claims?.ToList() ?? new List<Claim>();
            }

            return this.requestContextAccessor.ReceivedContext?.User?.Claims?
                .Select(e => new Claim(e.Key, e.Value))
                .ToList() ?? new List<Claim>();
        }
    }
}