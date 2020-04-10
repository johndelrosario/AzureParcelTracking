using AzureFromTheTrenches.Commanding.Abstractions;
using AzureParcelTracking.Commands.Models;
using System;

namespace AzureParcelTracking.Commands
{
    public class AddConsignmentCommand : ICommand<Consignment>
    {
        public NewConsignment Consignment { get; set; }

        public Guid CreatedByUserId { get; set; }
    }
}
