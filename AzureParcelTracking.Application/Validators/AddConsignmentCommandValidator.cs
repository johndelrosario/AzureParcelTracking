using AzureParcelTracking.Commands;
using FluentValidation;

namespace AzureParcelTracking.Application.Validators
{
    internal class AddConsignmentCommandValidator : AbstractValidator<AddConsignmentCommand>
    {
        public AddConsignmentCommandValidator()
        {
            RuleFor(cmd => cmd.CreatedByUserId).NotEmpty();
            RuleFor(cmd => cmd.Consignment).NotNull().SetValidator(new ConsignmentValidator());
        }
    }
}
