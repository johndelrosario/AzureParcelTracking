using AutoMapper;
using AzureFromTheTrenches.Commanding.Abstractions;
using AzureParcelTracking.Application.Repositories.Interfaces;
using AzureParcelTracking.Commands;
using AzureParcelTracking.Commands.Models;
using System.Threading.Tasks;

namespace AzureParcelTracking.Application.Handlers
{
    internal class GetConsignmentHandler : ICommandHandler<GetConsignmentQuery, Consignment>
    {
        private readonly IConsignmentRepository _consignmentRepository;
        private readonly IMapper _mapper;

        public GetConsignmentHandler(IConsignmentRepository consignmentRepository, IMapper mapper)
        {
            _consignmentRepository = consignmentRepository;
            _mapper = mapper;
        }

        public async Task<Consignment> ExecuteAsync(GetConsignmentQuery command, Consignment previousResult)
        {
            var consignment = await _consignmentRepository.Get(command.Id);

            return _mapper.Map<Consignment>(consignment);
        }
    }
}
