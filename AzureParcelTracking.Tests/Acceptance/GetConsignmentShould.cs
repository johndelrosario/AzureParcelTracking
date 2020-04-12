using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AzureFromTheTrenches.Commanding.Abstractions;
using AzureParcelTracking.Application.Models;
using AzureParcelTracking.Application.Repositories.Interfaces;
using AzureParcelTracking.Commands;
using AzureParcelTracking.Commands.Enums;
using AzureParcelTracking.Commands.Models;
using FunctionMonkey.Testing;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NSubstitute;
using Xbehave;
using Xunit;

namespace AzureParcelTracking.Tests.Acceptance
{
    public class GetConsignmentShould : AbstractAcceptanceTest
    {
        private readonly Guid _withDeliveredTrackingConsignmentId = Guid.NewGuid();

        public override void BeforeServiceProviderBuild(IServiceCollection serviceCollection,
            ICommandRegistry commandRegistry)
        {
            base.BeforeServiceProviderBuild(serviceCollection, commandRegistry);

            var consignmentRepository = Substitute.For<IConsignmentRepository>();

            consignmentRepository.Get(Arg.Any<Guid>()).Returns(callInfo => Task.FromResult(new ConsignmentRecord
            {
                Id = callInfo.Arg<Guid>()
            }));

            consignmentRepository.Get(Arg.Is(_withDeliveredTrackingConsignmentId)).Returns(callInfo =>
                Task.FromResult(new ConsignmentRecord
                {
                    Id = callInfo.Arg<Guid>(),
                    TrackingRecords = new List<TrackingRecord>
                    {
                        new TrackingRecord
                        {
                            Id = Guid.NewGuid(), ConsignmentId = callInfo.Arg<Guid>(),
                            TrackingType = TrackingType.Accepted
                        }
                    }
                }));

            serviceCollection.Replace(new ServiceDescriptor(typeof(IConsignmentRepository), consignmentRepository));
        }

        [Scenario]
        public void ReturnAnExistingConsignment(Guid consignmentId, HttpResponse response)
        {
            "Given a consignment id".x(() => consignmentId = Guid.NewGuid());

            "When I request a consignment".x(async () => response = await ExecuteHttpAsync(new GetConsignmentQuery
            {
                Id = consignmentId
            }));

            "Then I receive the expected consignment".x(() =>
            {
                Assert.Equal((int) HttpStatusCode.OK, response.StatusCode);
                var consignment = response.GetJson<Consignment>();

                Assert.Equal(consignmentId, consignment.Id);
            });
        }

        [Scenario]
        public void ReturnAnExistingConsignmentWithTracking(Guid consignmentId, HttpResponse response)
        {
            "Given a consignment id".x(() => consignmentId = _withDeliveredTrackingConsignmentId);

            "When I request a consignment".x(async () => response = await ExecuteHttpAsync(new GetConsignmentQuery
            {
                Id = consignmentId,
                WithTracking = true
            }));

            "Then I receive the expected consignment with tracking details".x(() =>
            {
                Assert.Equal((int) HttpStatusCode.OK, response.StatusCode);
                var consignment = response.GetJson<Consignment>();

                Assert.Equal(consignmentId, consignment.Id);
                Assert.NotNull(consignment.TrackingRecords);
                Assert.True(consignment.TrackingRecords.Count > 0);
            });
        }

        [Scenario]
        public void ReturnNotFoundWhenConsignmentDoesNotExist(Guid consignmentId, HttpResponse response)
        {
            var scaffold = new AcceptanceTestScaffold();
            scaffold.Setup();

            "Given a consignment id that does not exist".x(() => consignmentId = Guid.NewGuid());

            "When I request a consignment".x(async () => response = await scaffold.ExecuteHttpAsync(
                new GetConsignmentQuery
                {
                    Id = consignmentId
                }));

            "Then I receive a not found status code".x(() =>
                Assert.Equal((int) HttpStatusCode.NotFound, response.StatusCode));
        }
    }
}