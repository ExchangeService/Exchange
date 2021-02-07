using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;


namespace Exchange.Shared.Security
{
    public interface IUserDetailsProvider
    {
        Task<ClaimsPrincipal?> GetAsync();

        Task<string?> GetClaimValueAsync(string claim);

        Task<string?> GetEmailAsync();

        Task<string?> GetFirstNameAsync();

        Task<string?> GetPhoneNumberAsync();

        Task<string> GetRequiredUserNameAsync();

        Task<string?> GetRoleAsync();

        Task<string?> GetSecondNameAsync();

        Task<string?> GetTokenAsync();

        Task<string?> GetUserNameAsync();

        Task RelayAuthorizationAsync(Dictionary<string, string> headers);
    }
}