using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AzureParcelTracking.Application.Models;

namespace AzureParcelTracking.Application.Repositories.Interfaces
{
    public interface IRepository<TType> where TType : BaseRecord, new()
    {
        Task<TType> Get(Guid id);

        Task<IReadOnlyList<TType>> Get(Func<TType, bool> condition);

        Task<Guid> Add(TType item, Guid userId);

        Task Update(TType item, Guid userId);

        Task<bool> Delete(TType item, Guid userId);

        void LoadWith(Expression<Func<TType, object>> expression);
    }
}