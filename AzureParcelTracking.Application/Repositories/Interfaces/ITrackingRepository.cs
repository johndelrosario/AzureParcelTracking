using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AzureParcelTracking.Application.Models;

namespace AzureParcelTracking.Application.Repositories.Interfaces
{
    internal interface ITrackingRepository : IRepository<TrackingRecord>
    {
        Task<IReadOnlyList<TrackingRecord>> GetByConsignmentId(Guid consignmentId);

        Task<IReadOnlyList<TrackingRecord>> GetByConsignmentId(IReadOnlyList<Guid> consignmentIds);
    }
}