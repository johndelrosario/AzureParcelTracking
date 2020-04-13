using System.Security.Claims;
using System.Threading.Tasks;
using AzureParcelTracking.Application.Helpers.Interface;
using FunctionMonkey.Abstractions;

namespace AzureParcelTracking
{
    public class BearerTokenValidator : ITokenValidator
    {
        private readonly IJwtHelper _jwtHelper;

        public BearerTokenValidator(IJwtHelper jwtHelper)
        {
            _jwtHelper = jwtHelper;
        }

        public Task<ClaimsPrincipal> ValidateAsync(string authorizationHeader)
        {
            if (!authorizationHeader.StartsWith("Bearer "))
                return null;

            var bearerToken = authorizationHeader.Substring("Bearer ".Length);

            var result = _jwtHelper.GetClaimsPrincipal(bearerToken);

            return Task.FromResult(result);
        }
    }
}