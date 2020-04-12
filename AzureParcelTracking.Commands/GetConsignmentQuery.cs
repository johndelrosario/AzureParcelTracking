using AzureFromTheTrenches.Commanding.Abstractions;
using AzureParcelTracking.Commands.Models;
using System;

namespace AzureParcelTracking.Commands
{
    public class GetConsignmentQuery : ICommand<Consignment>
    {
        public Guid Id { get; set; }

        public bool WithTracking { get; set; }
    }
}
