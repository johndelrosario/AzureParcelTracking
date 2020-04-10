using System;

namespace AzureParcelTracking.Application.Models
{
    internal class TrackingRecord : BaseRecord
    {
        public Guid ConsignmentId { get; set; }

        public AddressRecord Address { get; set; }
    }
}