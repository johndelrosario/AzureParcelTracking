using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using AzureParcelTracking.Application.Models;
using AzureParcelTracking.Application.Repositories.Interfaces;

namespace AzureParcelTracking.Application.Repositories.Implementation
{
    internal class ConsignmentRepository : BaseRepository<ConsignmentRecord>, IConsignmentRepository
    {
        private readonly ITrackingRepository _trackingRepository;
        private readonly IMapper _mapper;

        public ConsignmentRepository(ITrackingRepository trackingRepository, IMapper mapper)
        {
            _trackingRepository = trackingRepository;
            _mapper = mapper;
        }

        protected override ConsignmentRecord RunLoadWith(ConsignmentRecord result)
        {
            var newResult = _mapper.Map<ConsignmentRecord>(result);

            foreach (var memberExpression in LoadWithExpressions)
                if (memberExpression?.Member?.Name == nameof(ConsignmentRecord.TrackingRecords))
                    GetTrackingRecords(new List<ConsignmentRecord> {newResult});

            return newResult;
        }

        protected override IReadOnlyList<ConsignmentRecord> RunLoadWith(IReadOnlyList<ConsignmentRecord> results)
        {
            var newResults = results.Select(result => _mapper.Map<ConsignmentRecord>(result)).ToList();

            foreach (var memberExpression in LoadWithExpressions)
                if (memberExpression?.Member?.Name == nameof(ConsignmentRecord.TrackingRecords))
                    GetTrackingRecords(newResults);

            return newResults;
        }

        private async void GetTrackingRecords(IReadOnlyList<ConsignmentRecord> consignmentRecords)
        {
            var consignmentRecordIds = consignmentRecords.Select(record => record.Id).Distinct();
            var trackingRecords = await _trackingRepository.GetByConsignmentId(consignmentRecordIds.ToList());

            foreach (var consignmentRecord in consignmentRecords)
                consignmentRecord.TrackingRecords = trackingRecords
                    .Where(trackingRecord => trackingRecord.ConsignmentId == consignmentRecord.Id)
                    .ToList();
        }
    }
}