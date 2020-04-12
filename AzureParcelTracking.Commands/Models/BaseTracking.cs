using System;
using AzureParcelTracking.Commands.Enums;

namespace AzureParcelTracking.Commands.Models
{
    public class BaseTracking
    {
        public Guid ConsignmentId { get; set; }

        public Address Address { get; set; }

        public TrackingType TrackingType { get; set; }
    }
}