using System;
using System.Threading.Tasks;
using AzureFromTheTrenches.Commanding.Abstractions;
using AzureParcelTracking.Commands;
using AzureParcelTracking.Commands.Models;

namespace AzureParcelTracking.Application.Handlers
{
    internal class AddTrackingHandler : ICommandHandler<AddTrackingCommand, Tracking>
    {
        public Task<Tracking> ExecuteAsync(AddTrackingCommand command, Tracking previousResult)
        {
            throw new NotImplementedException();
        }
    }
}