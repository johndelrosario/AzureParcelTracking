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
    internal class AddConsignmentHandler : ICommandHandler<AddConsignmentCommand, Consignment>
    {
        private readonly IConsignmentRepository _consignmentRepository;
        private readonly IMapper _mapper;

        public AddConsignmentHandler(IConsignmentRepository consignmentRepository, IMapper mapper)
        {
            consignmentRepository.LoadWith(record => record.TrackingRecords);

            _consignmentRepository = consignmentRepository;
            _mapper = mapper;
        }

        public async Task<Consignment> ExecuteAsync(AddConsignmentCommand command,
            Consignment previousConsignment)
        {
            var consignment = _mapper.Map<ConsignmentRecord>(command.Consignment);

            await AddConsignmentToRepository(consignment, Guid.Parse(command.CreatedByUserId));

            return _mapper.Map<Consignment>(consignment);
        }

        private async Task AddConsignmentToRepository(ConsignmentRecord consignment, Guid userId)
        {
            await _consignmentRepository.Add(consignment, userId);
        }
    }
}