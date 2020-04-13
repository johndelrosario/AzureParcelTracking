using System;
using AzureFromTheTrenches.Commanding.Abstractions;
using AzureParcelTracking.Commands.Models;

namespace AzureParcelTracking.Commands
{
    public class AddTrackingCommand : ICommand<Tracking>
    {
        public NewTracking Tracking { get; set; }

        [SecurityProperty]
        public string CreatedByUserId { get; set; }
    }
}