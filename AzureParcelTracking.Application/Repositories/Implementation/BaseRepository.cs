using AzureParcelTracking.Application.Exceptions;
using AzureParcelTracking.Application.Models;
using AzureParcelTracking.Application.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureParcelTracking.Application.Repositories.Implementation
{
    internal class BaseRepository<TType> : IRepository<TType> where TType : BaseRecord, new()
    {
        private const string ITEM_DOES_NOT_EXISTS_MESSAGE = "Item does not exists";
        private readonly List<TType> _collection = new List<TType>();

        public virtual Task<Guid> Add(TType item, Guid userId)
        {
            item.Id = Guid.NewGuid();
            item.CreatedAtUtc = DateTime.UtcNow;
            item.CreatedByUserId = userId;

            _collection.Add(item);

            return Task.FromResult(item.Id);
        }

        public virtual Task<bool> Delete(TType item, Guid userId)
        {
            TryGetTargetItem(item);

            item.DeletedAtUtc = DateTime.UtcNow;
            item.DeletedByUserId = userId;
            item.IsDeleted = true;

            return Task.FromResult(true);
        }

        public virtual Task<TType> Get(Guid id)
        {
            var targetItem = _collection.FirstOrDefault(collectionItem => collectionItem.Id == id);

            return Task.FromResult(TryGetTargetItem(id));
        }

        public virtual Task Update(TType item, Guid userId)
        {
            var targetItem = TryGetTargetItem(item);

            item.UpdatedAtUtc = DateTime.UtcNow;
            item.UpdatedByUserId = userId;

            _collection[_collection.IndexOf(targetItem)] = item;

            return Task.CompletedTask;
        }

        private TType TryGetTargetItem(TType item) => TryGetTargetItem(item.Id);

        private TType TryGetTargetItem(Guid id)
        {
            var targetItem = _collection.FirstOrDefault(collectionItem => collectionItem.Id == id);

            if (targetItem == null)
                throw new ItemNotFoundException(ITEM_DOES_NOT_EXISTS_MESSAGE);
            else
                return targetItem;
        }
    }
}
