using System.Collections.Generic;
using System.Linq;
using AzureParcelTracking.Application.Models;
using AzureParcelTracking.Application.Repositories.Interfaces;

namespace AzureParcelTracking.Application.Repositories.Implementation
{
    internal class ConsignmentRepository : BaseRepository<ConsignmentRecord>, IConsignmentRepository
    {
        private readonly ITrackingRepository _trackingRepository;

        public ConsignmentRepository(ITrackingRepository trackingRepository)
        {
            _trackingRepository = trackingRepository;
        }

        protected override void RunLoadWith(ConsignmentRecord result)
        {
            foreach (var memberExpression in LoadWithExpressions)
                if (memberExpression?.Member?.Name == nameof(ConsignmentRecord.TrackingRecords))
                    GetTrackingRecords(new List<ConsignmentRecord> {result});
        }

        protected override void RunLoadWith(IReadOnlyList<ConsignmentRecord> results)
        {
            foreach (var memberExpression in LoadWithExpressions)
                if (memberExpression?.Member?.Name == nameof(ConsignmentRecord.TrackingRecords))
                    GetTrackingRecords(results);
        }

        private void GetTrackingRecords(IReadOnlyList<ConsignmentRecord> consignmentRecords)
        {
            var consignmentRecordIds = consignmentRecords.Select(record => record.Id).Distinct();
            var trackingRecords = _trackingRepository.GetByConsignmentId(consignmentRecordIds.ToList());

            foreach (var consignmentRecord in consignmentRecords)
                consignmentRecord.TrackingRecords = trackingRecords
                    .Where(trackingRecord => trackingRecord.ConsignmentId == consignmentRecord.Id).ToList();
        }
    }
}