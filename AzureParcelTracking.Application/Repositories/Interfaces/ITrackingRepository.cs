using System;
using System.Collections.Generic;
using System.Text;
using AzureParcelTracking.Application.Models;

namespace AzureParcelTracking.Application.Repositories.Interfaces
{
    interface ITrackingRepository : IRepository<TrackingRecord>
    {
    }
}
