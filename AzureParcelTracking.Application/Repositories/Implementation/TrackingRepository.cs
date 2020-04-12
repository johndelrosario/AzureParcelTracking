using System;
using System.Collections.Generic;
using System.Linq;
using AzureParcelTracking.Application.Models;
using AzureParcelTracking.Application.Repositories.Interfaces;

namespace AzureParcelTracking.Application.Repositories.Implementation
{
    internal class TrackingRepository : BaseRepository<TrackingRecord>, ITrackingRepository
    {
        public IReadOnlyList<TrackingRecord> GetByConsignmentId(Guid consignmentId)
        {
            var consignments = new List<TrackingRecord>();

            consignments.AddRange(Collection.Where(consignment => consignment.ConsignmentId == consignmentId));

            return consignments;
        }

        public IReadOnlyList<TrackingRecord> GetByConsignmentId(IReadOnlyList<Guid> consignmentIds)
        {
            var consignments = new List<TrackingRecord>();

            consignments.AddRange(Collection.Where(consignment => consignmentIds.Contains(consignment.ConsignmentId)));

            return consignments;
        }
    }
}