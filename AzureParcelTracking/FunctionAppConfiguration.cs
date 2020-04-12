using System.Net.Http;
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
                .AddFluentValidation()
                .DefaultHttpResponseHandler<HttpResponseHandler>()
                .Functions(functions =>
                    functions
                        .HttpRoute("/api/consignment/v1/add",
                            route => route.HttpFunction<AddConsignmentCommand>(HttpMethod.Post))
                        .HttpRoute("/api/consignment/v1/",
                            route => route.HttpFunction<GetConsignmentQuery>(HttpMethod.Get))
                        .HttpRoute("/api/tracking/v1/add",
                            route => route.HttpFunction<AddTrackingCommand>(HttpMethod.Post))
                );
        }
    }
}