using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AzureParcelTracking.Application.Exceptions;
using AzureParcelTracking.Application.Models;
using AzureParcelTracking.Application.Repositories.Interfaces;

namespace AzureParcelTracking.Application.Repositories.Implementation
{
    internal abstract class BaseRepository<TType> : IRepository<TType> where TType : BaseRecord, new()
    {
        private const string ItemDoesNotExistsMessage = "Item does not exists";
        private readonly List<Expression<Func<TType, object>>> _loadWithExpressions;
        protected static readonly List<TType> Collection = new List<TType>();

        protected BaseRepository()
        {
            _loadWithExpressions = new List<Expression<Func<TType, object>>>();
        }

        public virtual Task<Guid> Add(TType item, Guid userId)
        {
            item.Id = Guid.NewGuid();
            item.CreatedAtUtc = DateTime.UtcNow;
            item.CreatedByUserId = userId;

            Collection.Add(item);

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
            var targetItem = TryGetTargetItem(id);

            RunLoadWith(targetItem);

            return Task.FromResult(targetItem);
        }

        public virtual Task<IReadOnlyList<TType>> Get(Func<TType, bool> predicate)
        {
            var targetItems = new List<TType>();

            targetItems.AddRange(Collection.Where(predicate));

            return Task.FromResult<IReadOnlyList<TType>>(targetItems);
        }

        public virtual Task Update(TType item, Guid userId)
        {
            var targetItem = TryGetTargetItem(item);

            item.UpdatedAtUtc = DateTime.UtcNow;
            item.UpdatedByUserId = userId;

            Collection[Collection.IndexOf(targetItem)] = item;

            return Task.CompletedTask;
        }

        public void LoadWith(Expression<Func<TType, object>> expression)
        {
            if (expression?.Body != null) _loadWithExpressions.Add(expression);
        }

        protected virtual void RunLoadWith(TType result)
        {
            // Optional implementation
        }

        protected virtual void RunLoadWith(IReadOnlyList<TType> results)
        {
            // Optional implementation
        }

        private TType TryGetTargetItem(TType item)
        {
            return TryGetTargetItem(item.Id);
        }

        private TType TryGetTargetItem(Guid id)
        {
            var targetItem = Collection.FirstOrDefault(collectionItem => collectionItem.Id == id);

            if (targetItem == null)
                throw new ItemNotFoundException(ItemDoesNotExistsMessage);
            return targetItem;
        }

        protected IEnumerable<MemberExpression> LoadWithExpressions =>
            _loadWithExpressions
                .Select(expression => expression.Body as MemberExpression)
                .Where(expression => expression != null);
    }
}