using System;
using System.Collections.Generic;
using System.Text;

namespace AzureParcelTracking.Commands.Models
{
    public class Tracking : BaseTracking
    {
        public DateTime CreatedAtUtc { get; set; }

        public Guid CreatedByUserId { get; set; }

        public Guid Id { get; set; }
    }
}
