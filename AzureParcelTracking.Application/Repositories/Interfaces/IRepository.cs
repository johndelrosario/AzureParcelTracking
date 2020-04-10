using AzureParcelTracking.Application.Models;
using System;
using System.Threading.Tasks;

namespace AzureParcelTracking.Application.Repositories.Interfaces
{
    internal interface IRepository<TType> where TType : BaseRecord, new()
    {
        Task<TType> Get(Guid id);

        Task<Guid> Add(TType item, Guid userId);

        Task Update(TType item, Guid userId);

        Task<bool> Delete(TType item, Guid userId);
    }
}
