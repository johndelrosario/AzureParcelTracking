using System;
using System.Collections.Generic;

namespace AzureParcelTracking.Commands.Models
{
    public class Consignment : BaseConsignment
    {
        public DateTime CreatedAtUtc { get; set; }

        public Guid CreatedByUserId { get; set; }

        public Guid Id { get; set; }

        public IReadOnlyList<Tracking> TrackingRecords { get; set; }
    }
}