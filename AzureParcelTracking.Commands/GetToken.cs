using AzureFromTheTrenches.Commanding.Abstractions;

namespace AzureParcelTracking.Commands
{
    public class GetToken : ICommand<string>
    {
        public string Username { get; set; }

        public string Password { get; set; }
    }
}