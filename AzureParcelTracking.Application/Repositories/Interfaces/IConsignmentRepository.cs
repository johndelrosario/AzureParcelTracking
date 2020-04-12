using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AzureParcelTracking.Application.Models;

namespace AzureParcelTracking.Application.Repositories.Interfaces
{
    internal interface IConsignmentRepository : IRepository<ConsignmentRecord>
    {
    }
}
