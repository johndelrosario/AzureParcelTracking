using System.Collections.Generic;

namespace AzureParcelTracking.Application.Models
{
    internal class ConsignmentRecord : BaseRecord
    {
        public AddressRecord Receiver { get; set; }

        public AddressRecord Sender { get; set; }

        public IReadOnlyList<TrackingRecord> TrackingRecords { get; set; }
    }
}
