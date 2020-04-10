using System;

namespace AzureParcelTracking.Application.Models
{
    internal class BaseRecord
    {
        public DateTime CreatedAtUtc { get; set; }
        public Guid CreatedByUserId { get; set; }
        public DateTime DeletedAtUtc { get; set; }
        public Guid DeletedByUserId { get; set; }
        public Guid Id { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime UpdatedAtUtc { get; set; }
        public Guid UpdatedByUserId { get; set; }
    }
}
