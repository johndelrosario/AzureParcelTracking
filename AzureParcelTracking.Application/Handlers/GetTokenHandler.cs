using System.Threading.Tasks;
using AzureFromTheTrenches.Commanding.Abstractions;
using AzureParcelTracking.Application.Helpers.Interface;
using AzureParcelTracking.Application.Repositories.Interfaces;
using AzureParcelTracking.Commands;

namespace AzureParcelTracking.Application.Handlers
{
    internal class GetTokenHandler : ICommandHandler<GetToken, string>
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtHelper _jwtHelper;

        public GetTokenHandler(IUserRepository userRepository, IJwtHelper jwtHelper)
        {
            _userRepository = userRepository;
            _jwtHelper = jwtHelper;
        }

        public async Task<string> ExecuteAsync(GetToken command, string previousResult)
        {
            var user = await _userRepository.GetByCredentials(command.Username, command.Password);
            var token = _jwtHelper.GetToken(user.Id);

            return token;
        }
    }
}