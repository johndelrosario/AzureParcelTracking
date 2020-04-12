using System;
using System.Collections.Generic;
using AzureParcelTracking.Application.Models;

namespace AzureParcelTracking.Application.Repositories.Interfaces
{
    internal interface ITrackingRepository : IRepository<TrackingRecord>
    {
        IReadOnlyList<TrackingRecord> GetByConsignmentId(Guid consignmentId);

        IReadOnlyList<TrackingRecord> GetByConsignmentId(IReadOnlyList<Guid> consignmentIds);
    }
}