using System;
using System.Threading.Tasks;
using AutoMapper;
using AzureFromTheTrenches.Commanding.Abstractions;
using AzureParcelTracking.Application.Models;
using AzureParcelTracking.Application.Repositories.Interfaces;
using AzureParcelTracking.Commands;
using AzureParcelTracking.Commands.Models;

namespace AzureParcelTracking.Application.Handlers
{
    internal class AddTrackingHandler : ICommandHandler<AddTrackingCommand, Tracking>
    {
        private readonly ITrackingRepository _trackingRepository;
        private readonly IMapper _mapper;

        public AddTrackingHandler(ITrackingRepository trackingRepository, IMapper mapper)
        {
            _trackingRepository = trackingRepository;
            _mapper = mapper;
        }

        public async Task<Tracking> ExecuteAsync(AddTrackingCommand command, Tracking previousResult)
        {
            var tracking = _mapper.Map<TrackingRecord>(command.Tracking);

            await AddTrackingToRepository(tracking, command.CreatedByUserId);

            return _mapper.Map<Tracking>(tracking);
        }

        private async Task AddTrackingToRepository(TrackingRecord tracking, Guid userId)
        {
            await _trackingRepository.Add(tracking, userId);
        }
    }
}