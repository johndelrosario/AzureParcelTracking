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
    public class AddConsignmentShould : AbstractAcceptanceTest
    {
        [Scenario]
        public void ReturnAddedConsignment(NewConsignment newConsignment, HttpResponse response,
            Consignment savedConsignment)
        {
            "Given a new consignment".x(() => newConsignment = new NewConsignment
            {
                Sender = new Address
                {
                    Address1 = "My Address 1",
                    Address2 = "My Address 2",
                    Name = "Sender Name",
                    Phone = "Sender Phone",
                    Postcode = "Sender Postcode"
                },
                Receiver = new Address
                {
                    Address1 = "My Address 1",
                    Address2 = "My Address 2",
                    Name = "Receiver Name",
                    Phone = "Receiver Phone",
                    Postcode = "Receiver Postcode"
                }
            });

            "When I submit the consignment".x(async () => response = await ExecuteHttpAsync(new AddConsignmentCommand
            {
                CreatedByUserId = Guid.NewGuid().ToString(),
                Consignment = newConsignment
            }));

            "Then I receive an OK result and a consignment".x(() =>
            {
                Assert.Equal((int) HttpStatusCode.OK, response.StatusCode);
                savedConsignment = response.GetJson<Consignment>();
                Assert.NotNull(savedConsignment);
            });

            "And a consignment has a record id".x(() => Assert.NotEqual(Guid.Empty, savedConsignment.Id));

            "And it has a created date".x(() => Assert.NotEqual(default, savedConsignment.CreatedAtUtc));

            "And it has matching sender details".x(() =>
            {
                Assert.Equal(newConsignment.Sender.Name, savedConsignment.Sender.Name);
                Assert.Equal(newConsignment.Sender.Address1, savedConsignment.Sender.Address1);
                Assert.Equal(newConsignment.Sender.Address2, savedConsignment.Sender.Address2);
                Assert.Equal(newConsignment.Sender.Postcode, savedConsignment.Sender.Postcode);
                Assert.Equal(newConsignment.Sender.Phone, savedConsignment.Sender.Phone);
            });

            "And it has matching receiver details".x(() =>
            {
                Assert.Equal(newConsignment.Receiver.Name, savedConsignment.Receiver.Name);
                Assert.Equal(newConsignment.Receiver.Address1, savedConsignment.Receiver.Address1);
                Assert.Equal(newConsignment.Receiver.Address2, savedConsignment.Receiver.Address2);
                Assert.Equal(newConsignment.Receiver.Postcode, savedConsignment.Receiver.Postcode);
                Assert.Equal(newConsignment.Receiver.Phone, savedConsignment.Receiver.Phone);
            });
        }

        [Scenario]
        public void ReturnBadRequestOnNoUserId(NewConsignment newConsignment, HttpResponse response)
        {
            "Given a new consignment".x(() => newConsignment =
                new NewConsignment
                {
                    Sender = new Address
                    {
                        Address1 = "My Address 1",
                        Address2 = "My Address 2",
                        Name = "Sender Name",
                        Phone = "Sender Phone",
                        Postcode = "Sender Postcode"
                    },
                    Receiver = new Address
                    {
                        Address1 = "My Address 1",
                        Address2 = "My Address 2",
                        Name = "Receiver Name",
                        Phone = "Receiver Phone",
                        Postcode = "Receiver Postcode"
                    }
                });

            "When I submit the consignment without a user id".x(async () => response = await ExecuteHttpAsync(
                new AddConsignmentCommand
                {
                    Consignment = newConsignment
                }));

            "Then I receive a bad request status code".x(() =>
                Assert.Equal((int) HttpStatusCode.BadRequest, response.StatusCode));

            "And a validation failure has been set for the properties".x(() =>
            {
                var validationResult = response.GetJson<ValidationResult>();

                Assert.NotNull(validationResult);
                Assert.Contains(validationResult.Errors, error =>
                    error.Property == "CreatedByUserId");
            });
        }

        [Scenario]
        public void ReturnBadRequestOnInvalidConsignmentSender(NewConsignment newConsignment, HttpResponse response)
        {
            "Given a new consignment with an empty sender name, address1, phone and post code".x(() => newConsignment =
                new NewConsignment
                {
                    Sender = new Address
                    {
                        Address1 = string.Empty,
                        Address2 = "My Address 2",
                        Name = string.Empty,
                        Phone = string.Empty,
                        Postcode = string.Empty
                    },
                    Receiver = new Address
                    {
                        Address1 = "My Address 1",
                        Address2 = "My Address 2",
                        Name = "Receiver Name",
                        Phone = "Receiver Phone",
                        Postcode = "Receiver Postcode"
                    }
                });

            "When I submit the consignment".x(async () => response = await ExecuteHttpAsync(new AddConsignmentCommand
            {
                CreatedByUserId = Guid.NewGuid().ToString(),
                Consignment = newConsignment
            }));

            "Then I receive a bad request status code".x(() =>
                Assert.Equal((int) HttpStatusCode.BadRequest, response.StatusCode));

            "And a validation failure has been set for the properties".x(() =>
            {
                var validationResult = response.GetJson<ValidationResult>();

                Assert.NotNull(validationResult);
                Assert.Contains(validationResult.Errors, error =>
                    error.Property == "Consignment.Sender.Name");
                Assert.Contains(validationResult.Errors, error =>
                    error.Property == "Consignment.Sender.Address1");
                Assert.Contains(validationResult.Errors, error =>
                    error.Property == "Consignment.Sender.Phone");
                Assert.Contains(validationResult.Errors, error =>
                    error.Property == "Consignment.Sender.Postcode");
            });
        }

        [Scenario]
        public void ReturnBadRequestOnInvalidConsignmentReceiver(NewConsignment newConsignment, HttpResponse response)
        {
            "Given a new consignment with an empty receiver name, address1, phone and post code".x(() =>
                newConsignment =
                    new NewConsignment
                    {
                        Sender = new Address
                        {
                            Address1 = "My Address 1",
                            Address2 = "My Address 2",
                            Name = "Sender Name",
                            Phone = "Sender Phone",
                            Postcode = "Sender Postcode"
                        },
                        Receiver = new Address
                        {
                            Address1 = string.Empty,
                            Address2 = "My Address 2",
                            Name = string.Empty,
                            Phone = string.Empty,
                            Postcode = string.Empty
                        }
                    });

            "When I submit the consignment".x(async () => response = await ExecuteHttpAsync(new AddConsignmentCommand
            {
                CreatedByUserId = Guid.NewGuid().ToString(),
                Consignment = newConsignment
            }));

            "Then I receive a bad request status code".x(() =>
                Assert.Equal((int) HttpStatusCode.BadRequest, response.StatusCode));

            "And a validation failure has been set for the properties".x(() =>
            {
                var validationResult = response.GetJson<ValidationResult>();

                Assert.NotNull(validationResult);
                Assert.Contains(validationResult.Errors, error =>
                    error.Property == "Consignment.Receiver.Name");
                Assert.Contains(validationResult.Errors, error =>
                    error.Property == "Consignment.Receiver.Address1");
                Assert.Contains(validationResult.Errors, error =>
                    error.Property == "Consignment.Receiver.Phone");
                Assert.Contains(validationResult.Errors, error =>
                    error.Property == "Consignment.Receiver.Postcode");
            });
        }

        [Scenario]
        public void InsertIntoConsignmentRepository(NewConsignment newConsignment, HttpResponse response)
        {
            ConsignmentRecord savedConsignment = null;

            "Given a new consignment".x(() => newConsignment = new NewConsignment
            {
                Sender = new Address
                {
                    Address1 = "My Address 1",
                    Address2 = "My Address 2",
                    Name = "Sender Name",
                    Phone = "Sender Phone",
                    Postcode = "Sender Postcode"
                },
                Receiver = new Address
                {
                    Address1 = "My Address 1",
                    Address2 = "My Address 2",
                    Name = "Receiver Name",
                    Phone = "Receiver Phone",
                    Postcode = "Receiver Postcode"
                }
            });

            "When i submit the consignment".x(async () => response = await ExecuteHttpAsync(new AddConsignmentCommand
            {
                CreatedByUserId = Guid.NewGuid().ToString(),
                Consignment = newConsignment
            }));

            "Then the consignment is available in the repository".x(async () =>
            {
                var returnedConsignment = response.GetJson<Consignment>();

                var consignmentRepository = ServiceProvider.GetRequiredService<IConsignmentRepository>();
                savedConsignment = await consignmentRepository.Get(returnedConsignment.Id);
            });

            "And a consignment has a record id and created date".x(() =>
            {
                Assert.NotEqual(Guid.Empty, savedConsignment.Id);
                Assert.NotEqual(default, savedConsignment.CreatedAtUtc);
            });

            "And it has matching sender details".x(() =>
            {
                Assert.Equal(newConsignment.Sender.Name, savedConsignment.Sender.Name);
                Assert.Equal(newConsignment.Sender.Address1, savedConsignment.Sender.Address1);
                Assert.Equal(newConsignment.Sender.Address2, savedConsignment.Sender.Address2);
                Assert.Equal(newConsignment.Sender.Postcode, savedConsignment.Sender.Postcode);
                Assert.Equal(newConsignment.Sender.Phone, savedConsignment.Sender.Phone);
            });

            "And it has matching receiver details".x(() =>
            {
                Assert.Equal(newConsignment.Receiver.Name, savedConsignment.Receiver.Name);
                Assert.Equal(newConsignment.Receiver.Address1, savedConsignment.Receiver.Address1);
                Assert.Equal(newConsignment.Receiver.Address2, savedConsignment.Receiver.Address2);
                Assert.Equal(newConsignment.Receiver.Postcode, savedConsignment.Receiver.Postcode);
                Assert.Equal(newConsignment.Receiver.Phone, savedConsignment.Receiver.Phone);
            });
        }
    }
}