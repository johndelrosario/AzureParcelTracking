using System;
using System.Threading.Tasks;
using AzureParcelTracking.Application.Models;

namespace AzureParcelTracking.Application.Repositories.Interfaces
{
    public interface IUserRepository : IRepository<UserRecord>
    {
        Task<Guid> Add(string username, string password);

        Task<UserRecord> GetByCredentials(string username, string password);
    }
}