using AzureParcelTracking.Commands;
using FluentValidation;

namespace AzureParcelTracking.Application.Validators
{
    internal class GetConsignmentQueryValidator : AbstractValidator<GetConsignmentQuery>
    {
        public GetConsignmentQueryValidator()
        {
            RuleFor(cmd => cmd.Id).NotEmpty();
        }
    }
}
