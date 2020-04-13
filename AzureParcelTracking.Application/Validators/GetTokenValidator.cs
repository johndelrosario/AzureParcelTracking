using AzureParcelTracking.Commands;
using FluentValidation;

namespace AzureParcelTracking.Application.Validators
{
    internal class GetTokenValidator : AbstractValidator<GetToken>
    {
        public GetTokenValidator()
        {
            RuleFor(user => user.Username).NotEmpty();
            RuleFor(user => user.Password).NotEmpty();
        }
    }
}
