using System;

namespace AzureParcelTracking.Commands.Models
{
    public class BaseTracking
    {
        public Guid ConsignmentId { get; set; }

        public Address CurrentAddress { get; set; }
    }
}