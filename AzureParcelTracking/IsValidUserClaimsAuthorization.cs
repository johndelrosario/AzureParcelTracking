using System;
using System.Security.Claims;
using System.Threading.Tasks;
using AzureParcelTracking.Application.Repositories.Interfaces;
using FunctionMonkey.Abstractions;

namespace AzureParcelTracking
{
    public class IsValidUserClaimsAuthorization : IClaimsPrincipalAuthorization
    {
        private readonly IUserRepository _userRepository;

        public IsValidUserClaimsAuthorization(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<bool> IsAuthorized(ClaimsPrincipal claimsPrincipal, string httpVerb, string requestUrl)
        {
            var subject = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (subject == null) return Task.FromResult(false);

            var userId = Guid.Parse(subject);

            try
            {
                return Task.FromResult(_userRepository.Get(userId) != null);
            }
            catch
            {
                return Task.FromResult(false);
            }
        }
    }
}