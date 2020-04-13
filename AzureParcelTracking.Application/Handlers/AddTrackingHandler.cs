using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AzureFromTheTrenches.Commanding.Abstractions;
using AzureParcelTracking.Application.Exceptions;
using AzureParcelTracking.Application.Models;
using AzureParcelTracking.Application.Repositories.Interfaces;
using AzureParcelTracking.Commands;
using AzureParcelTracking.Commands.Enums;
using AzureParcelTracking.Commands.Models;

namespace AzureParcelTracking.Application.Handlers
{
    internal class AddTrackingHandler : ICommandHandler<AddTrackingCommand, Tracking>
    {
        private const string ConsignmentAlreadyDeliveredMessage = "Consignment is already delivered";

        private readonly IConsignmentRepository _consignmentRepository;
        private readonly ITrackingRepository _trackingRepository;
        private readonly IMapper _mapper;

        public AddTrackingHandler(ITrackingRepository trackingRepository, IConsignmentRepository consignmentRepository,
            IMapper mapper)
        {
            _trackingRepository = trackingRepository;
            _consignmentRepository = consignmentRepository;
            _mapper = mapper;
        }

        public async Task<Tracking> ExecuteAsync(AddTrackingCommand command, Tracking previousResult)
        {
            _consignmentRepository.LoadWith(item => item.TrackingRecords);
            var consignment = await _consignmentRepository.Get(command.Tracking.ConsignmentId);

            if (consignment.TrackingRecords?.Any(record => record.TrackingType == TrackingType.Delivered) ?? false)
            {
                throw new ConsignmentAlreadyDeliveredException(ConsignmentAlreadyDeliveredMessage);
            }

            var tracking = _mapper.Map<TrackingRecord>(command.Tracking);

            await AddTrackingToRepository(tracking, Guid.Parse(command.CreatedByUserId));

            return _mapper.Map<Tracking>(tracking);
        }

        private async Task AddTrackingToRepository(TrackingRecord tracking, Guid userId)
        {
            await _trackingRepository.Add(tracking, userId);
        }
    }
}