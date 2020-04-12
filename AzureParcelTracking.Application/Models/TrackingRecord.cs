using System;
using AzureParcelTracking.Commands.Enums;

namespace AzureParcelTracking.Application.Models
{
    internal class TrackingRecord : BaseRecord
    {
        public Guid ConsignmentId { get; set; }

        public AddressRecord Address { get; set; }
        
        public TrackingType TrackingType { get; set; }
    }
}