using System.Net.Http;
using System.Security.Claims;
using AzureParcelTracking.Application;
using AzureParcelTracking.Commands;
using FunctionMonkey.Abstractions;
using FunctionMonkey.Abstractions.Builders;
using FunctionMonkey.FluentValidation;
using Newtonsoft.Json;

namespace AzureParcelTracking
{
    public class FunctionAppConfiguration : IFunctionAppConfiguration
    {
        public void Build(IFunctionHostBuilder builder)
        {
            builder
                .Setup((services, commandRegistry) =>
                {
                    services.AddApplication(commandRegistry);
                    JsonConvert.DefaultSettings = () => new JsonSerializerSettings
                        {NullValueHandling = NullValueHandling.Ignore};
                })
                .Authorization(authorization =>
                    authorization
                        .AuthorizationDefault(AuthorizationTypeEnum.TokenValidation)
                        .TokenValidator<BearerTokenValidator>()
                        .Claims(claims => claims
                            .MapClaimToCommandProperty<AddConsignmentCommand>(
                                ClaimTypes.NameIdentifier,
                                cmd => cmd.CreatedByUserId)
                            .MapClaimToCommandProperty<AddTrackingCommand>(
                                ClaimTypes.NameIdentifier,
                                cmd => cmd.CreatedByUserId))
                )
                .AddFluentValidation()
                .DefaultHttpResponseHandler<HttpResponseHandler>()
                .Functions(functions =>
                    functions
                        .HttpRoute("/api/token",
                            route => route.HttpFunction<GetToken>(AuthorizationTypeEnum.Anonymous, HttpMethod.Post))
                        .HttpRoute("/api/consignment/v1/add",
                            route => route.HttpFunction<AddConsignmentCommand>(AuthorizationTypeEnum.TokenValidation,
                                HttpMethod.Post))
                        .Options(options => options.ClaimsPrincipalAuthorization<IsValidUserClaimsAuthorization>())
                        .HttpRoute("/api/consignment/v1/",
                            route => route.HttpFunction<GetConsignmentQuery>(AuthorizationTypeEnum.Anonymous,
                                HttpMethod.Get))
                        .HttpRoute("/api/tracking/v1/add",
                            route => route.HttpFunction<AddTrackingCommand>(AuthorizationTypeEnum.TokenValidation,
                                HttpMethod.Post))
                        .Options(options => options.ClaimsPrincipalAuthorization<IsValidUserClaimsAuthorization>())
                );
        }
    }
}