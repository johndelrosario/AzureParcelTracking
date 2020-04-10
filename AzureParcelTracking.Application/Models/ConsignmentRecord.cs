namespace AzureParcelTracking.Application.Models
{
    internal class ConsignmentRecord : BaseRecord
    {
        public AddressRecord Receiver { get; set; }
        public AddressRecord Sender { get; set; }
    }
}
