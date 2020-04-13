using System;
using System.Security.Claims;

namespace AzureParcelTracking.Application.Helpers.Interface
{
    public interface IJwtHelper
    {
        string Issuer { get; }

        string Key { get; }

        int Timeout { get; }

        string GetToken(Guid userId);

        ClaimsPrincipal GetClaimsPrincipal(string bearerToken);
    }
}