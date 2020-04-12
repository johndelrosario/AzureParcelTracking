using System;

namespace AzureParcelTracking.Application.Exceptions
{
    public class ConsignmentAlreadyDeliveredException : Exception
    {
        public ConsignmentAlreadyDeliveredException(string message) : base(message)
        {
        }
    }
}