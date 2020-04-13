using AzureFromTheTrenches.Commanding;
using AzureFromTheTrenches.Commanding.Abstractions;
using AzureParcelTracking.Application.Exceptions;
using FunctionMonkey.Abstractions.Http;
using FunctionMonkey.Commanding.Abstractions.Validation;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace AzureParcelTracking
{
    public class HttpResponseHandler : IHttpResponseHandler
    {
        private static readonly Dictionary<Type, HttpStatusCode> ExceptionResponses = new Dictionary<Type, HttpStatusCode>
        {
            { typeof(ItemNotFoundException), HttpStatusCode.NotFound },
            { typeof(ConsignmentAlreadyDeliveredException), HttpStatusCode.Forbidden },
            { typeof(InvalidCredentialsException), HttpStatusCode.Unauthorized }
        };

        public Task<IActionResult> CreateResponse<TCommand, TResult>(TCommand command, TResult result) where TCommand : ICommand<TResult>
        {
            return null;
        }

        public Task<IActionResult> CreateResponse<TCommand>(TCommand command)
        {
            return null;
        }

        public Task<IActionResult> CreateResponseFromException<TCommand>(TCommand command, Exception ex) where TCommand : ICommand
        {
            var unwrappedException = ex;

            if(ex is CommandExecutionException cex)
            {
                unwrappedException = cex.InnerException;
            }

            if(ExceptionResponses.TryGetValue(unwrappedException.GetType(), out HttpStatusCode code))
            {
                return Task.FromResult((IActionResult) new StatusCodeResult((int)code));
            }

            return Task.FromResult((IActionResult) new InternalServerErrorResult());
        }

        public Task<IActionResult> CreateValidationFailureResponse<TCommand>(TCommand command, ValidationResult validationResult) where TCommand : ICommand
        {
            return null;
        }
    }
}
