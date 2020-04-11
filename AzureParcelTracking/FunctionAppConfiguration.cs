using AzureParcelTracking.Application;
using AzureParcelTracking.Commands;
using FunctionMonkey.Abstractions;
using FunctionMonkey.Abstractions.Builders;
using FunctionMonkey.FluentValidation;
using System.Net.Http;

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
                })
                .AddFluentValidation()
                .DefaultHttpResponseHandler<HttpResponseHandler>()
                .Functions(functions =>
                    functions
                        .HttpRoute("/api/consignment/v1/add", route => route.HttpFunction<AddConsignmentCommand>(HttpMethod.Post))
                        .HttpRoute("/api/consignment/v1/", route => route.HttpFunction<GetConsignmentQuery>(HttpMethod.Get))
                        .HttpRoute("/api/tracking/v1/add", route => route.HttpFunction<AddTrackingCommand>(HttpMethod.Post))
                );
        }
    }
}
