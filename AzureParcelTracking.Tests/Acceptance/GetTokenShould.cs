using System;
using System.Net;
using System.Threading.Tasks;
using AzureFromTheTrenches.Commanding.Abstractions;
using AzureParcelTracking.Application.Helpers.Interface;
using AzureParcelTracking.Application.Models;
using AzureParcelTracking.Application.Repositories.Interfaces;
using AzureParcelTracking.Commands;
using FunctionMonkey.Commanding.Abstractions.Validation;
using FunctionMonkey.Testing;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NSubstitute;
using Xbehave;
using Xunit;

namespace AzureParcelTracking.Tests.Acceptance
{
    public class GetTokenShould : AbstractAcceptanceTest
    {
        public override void BeforeServiceProviderBuild(IServiceCollection serviceCollection,
            ICommandRegistry commandRegistry)
        {
            base.BeforeServiceProviderBuild(serviceCollection, commandRegistry);

            var userRepository = Substitute.For<IUserRepository>();
            var jwtHelper = Substitute.For<IJwtHelper>();

            userRepository.GetByCredentials(Arg.Any<string>(), Arg.Any<string>()).Returns(callInfo =>
                Task.FromResult(new UserRecord
                {
                    Username = "Username",
                    Password = "IAmAHashedPassword",
                    Salt = "Salty"
                }));

            jwtHelper.GetToken(Arg.Any<Guid>()).Returns("RandomToken");

            serviceCollection.Replace(new ServiceDescriptor(typeof(IUserRepository), userRepository));
            serviceCollection.Replace(new ServiceDescriptor(typeof(IJwtHelper), jwtHelper));
        }

        [Scenario]
        public void ReturnATokenForAnExistingUser(string username, string password, HttpResponse response)
        {
            "Given a username and password".x(() =>
            {
                username = "Username";
                password = "Password";
            });

            "When I request a token".x(async () => response = await ExecuteHttpAsync(new GetToken
            {
                Username = username,
                Password = password
            }));

            "Then I receive a token".x(() =>
            {
                Assert.Equal((int) HttpStatusCode.OK, response.StatusCode);
                var token = response.GetJson<string>();

                Assert.NotEmpty(token);
            });
        }

        [Scenario]
        public void ReturnBadRequestOnNoUsernameAndPassword(string username, string password, HttpResponse response)
        {
            "Given a username and password".x(() =>
            {
                username = "";
                password = null;
            });

            "When I request a token".x(async () => response = await ExecuteHttpAsync(new GetToken
            {
                Username = username,
                Password = password
            }));

            "Then I receive a bad request status code".x(() =>
                Assert.Equal((int) HttpStatusCode.BadRequest, response.StatusCode));

            "And a validation failure has been set for the properties".x(() =>
            {
                var validationResult = response.GetJson<ValidationResult>();

                Assert.NotNull(validationResult);

                Assert.Contains(validationResult.Errors, error =>
                    error.Property == "Username");
                Assert.Contains(validationResult.Errors, error =>
                    error.Property == "Password");
            });
        }
    }
}