using AzureParcelTracking.Application.Models;
using AzureParcelTracking.Application.Repositories.Interfaces;

namespace AzureParcelTracking.Application.Repositories.Implementation
{
    internal class ConsignmentRepository : BaseRepository<ConsignmentRecord>, IConsignmentRepository
    {
    }
}
