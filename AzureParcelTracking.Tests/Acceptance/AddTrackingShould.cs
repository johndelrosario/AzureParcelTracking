using System;
using System.Net;
using AzureParcelTracking.Application.Models;
using AzureParcelTracking.Application.Repositories.Interfaces;
using AzureParcelTracking.Commands;
using AzureParcelTracking.Commands.Models;
using FunctionMonkey.Commanding.Abstractions.Validation;
using FunctionMonkey.Testing;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Xbehave;
using Xunit;

namespace AzureParcelTracking.Tests.Acceptance
{
    public class AddTrackingShould : AbstractAcceptanceTest
    {
        [Scenario]
        public void ReturnAddedTracking(NewTracking newTracking, HttpResponse response, Tracking savedTracking)
        {
            "Given a new tracking".x(() => newTracking = new NewTracking
            {
                Address = new Address
                {
                    Address1 = "My Address 1",
                    Address2 = "My Address 2",
                    Name = "Tracking Name",
                    Phone = "Tracking Phone",
                    Postcode = "Tracking Postcode"
                },
                ConsignmentId = Guid.NewGuid()
            });

            "When I submit the tracking".x(async () => response = await ExecuteHttpAsync(new AddTrackingCommand
            {
                CreatedByUserId = Guid.NewGuid(),
                Tracking = newTracking
            }));

            "Then I receive an OK result a tracking".x(() =>
            {
                Assert.Equal((int) HttpStatusCode.OK, response.StatusCode);
                savedTracking = response.GetJson<Tracking>();
                Assert.NotNull(savedTracking);
            });

            "And a tracking has a record id".x(() => Assert.NotEqual(Guid.Empty, savedTracking.Id));

            "And it has a created date".x(() => Assert.NotEqual(default, savedTracking.CreatedAtUtc));

            "And it has matching address address".x(() =>
            {
                Assert.Equal(newTracking.Address.Name, savedTracking.Address.Name);
                Assert.Equal(newTracking.Address.Address1, savedTracking.Address.Address1);
                Assert.Equal(newTracking.Address.Address2, savedTracking.Address.Address2);
                Assert.Equal(newTracking.Address.Postcode, savedTracking.Address.Postcode);
                Assert.Equal(newTracking.Address.Phone, savedTracking.Address.Phone);
            });
        }

        [Scenario]
        public void ReturnBadRequestOnNoUserId(NewTracking newTracking, HttpResponse response)
        {
            "Given a new tracking".x(() => newTracking = new NewTracking
            {
                Address = new Address
                {
                    Address1 = "My Address 1",
                    Address2 = "My Address 2",
                    Name = "Tracking Name",
                    Phone = "Tracking Phone",
                    Postcode = "Tracking Postcode"
                },
                ConsignmentId = Guid.NewGuid()
            });

            "When I submit the tracking without a user id".x(async () => response = await ExecuteHttpAsync(
                new AddTrackingCommand
                {
                    Tracking = newTracking
                }));

            "Then I receive a bad request status code".x(() =>
                Assert.Equal((int) HttpStatusCode.BadRequest, response.StatusCode));

            "And a validation failure has been set for the properties".x(() =>
            {
                var validationResult = response.GetJson<ValidationResult>();

                Assert.NotNull(validationResult);
                Assert.Contains(validationResult.Errors, error => error.Property == "CreatedByUserId");
            });
        }

        [Scenario]
        public void ReturnBadRequestOnNoConsignmentId(NewTracking newTracking, HttpResponse response)
        {
            "Given a new tracking".x(() => newTracking = new NewTracking
            {
                Address = new Address
                {
                    Address1 = "My Address 1",
                    Address2 = "My Address 2",
                    Name = "Tracking Name",
                    Phone = "Tracking Phone",
                    Postcode = "Tracking Postcode"
                }
            });

            "When I submit the tracking without a user id".x(async () => response = await ExecuteHttpAsync(
                new AddTrackingCommand
                {
                    CreatedByUserId = Guid.NewGuid(),
                    Tracking = newTracking
                }));

            "Then I receive a bad request status code".x(() =>
                Assert.Equal((int) HttpStatusCode.BadRequest, response.StatusCode));

            "And a validation failure has been set for the properties".x(() =>
            {
                var validationResult = response.GetJson<ValidationResult>();

                Assert.NotNull(validationResult);
                Assert.Contains(validationResult.Errors, error => error.Property == "Tracking.ConsignmentId");
            });
        }

        [Scenario]
        public void ReturnBadRequestOnInvalidAddress(NewTracking newTracking, HttpResponse response)
        {
            "Given a new tracking with an empty name, address1, phone and post code".x(() => newTracking =
                new NewTracking
                {
                    Address = new Address
                    {
                        Address1 = string.Empty,
                        Address2 = "My Address 2",
                        Name = string.Empty,
                        Phone = string.Empty,
                        Postcode = string.Empty
                    },
                    ConsignmentId = Guid.NewGuid()
                });

            "When I submit the tracking".x(async () => response = await ExecuteHttpAsync(
                new AddTrackingCommand
                {
                    CreatedByUserId = Guid.NewGuid(),
                    Tracking = newTracking
                }));

            "Then I receive a bad request status code".x(() =>
                Assert.Equal((int) HttpStatusCode.BadRequest, response.StatusCode));

            "And a validation failure has been set for the properties".x(() =>
            {
                var validationResult = response.GetJson<ValidationResult>();

                Assert.NotNull(validationResult);
                Assert.Contains(validationResult.Errors, error => error.Property == "Tracking.Address.Address1");
                Assert.Contains(validationResult.Errors, error => error.Property == "Tracking.Address.Postcode");
            });
        }

        [Scenario]
        public void InsertIntoTrackingRepository(NewTracking newTracking, HttpResponse response)
        {
            TrackingRecord savedTracking = null;

            "Given a new tracking".x(() => newTracking = new NewTracking
            {
                Address = new Address
                {
                    Address1 = "My Address 1",
                    Address2 = "My Address 2",
                    Name = "Tracking Name",
                    Phone = "Tracking Phone",
                    Postcode = "Tracking Postcode"
                },
                ConsignmentId = Guid.NewGuid()
            });

            "When I submit the tracking".x(async () => response = await ExecuteHttpAsync(
                new AddTrackingCommand
                {
                    CreatedByUserId = Guid.NewGuid(),
                    Tracking = newTracking
                }));

            "Then the tracking is available in the repository".x(async () =>
            {
                var returnedTracking = response.GetJson<Tracking>();

                var trackingRepository = ServiceProvider.GetRequiredService<ITrackingRepository>();

                savedTracking = await trackingRepository.Get(returnedTracking.Id);
            });

            "And a tracking has a record id, created date and consignment id".x(() =>
            {
                Assert.NotEqual(Guid.Empty, savedTracking.Id);
                Assert.NotEqual(default, savedTracking.CreatedAtUtc);
                Assert.NotEqual(Guid.Empty, savedTracking.ConsignmentId);
            });

            "And it has matching address".x(() =>
            {
                Assert.Equal(newTracking.Address.Name, savedTracking.Address.Name);
                Assert.Equal(newTracking.Address.Address1, savedTracking.Address.Address1);
                Assert.Equal(newTracking.Address.Address2, savedTracking.Address.Address2);
                Assert.Equal(newTracking.Address.Postcode, savedTracking.Address.Postcode);
                Assert.Equal(newTracking.Address.Phone, savedTracking.Address.Phone);
            });
        }
    }
}