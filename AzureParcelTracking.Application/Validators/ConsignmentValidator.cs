using AzureParcelTracking.Commands.Models;
using FluentValidation;

namespace AzureParcelTracking.Application.Validators
{
    internal class ConsignmentValidator : AbstractValidator<NewConsignment>
    {
        public ConsignmentValidator()
        {
            RuleFor(consignment => consignment.Sender).NotNull().SetValidator(new AddressValidator());
            RuleFor(consignment => consignment.Receiver).NotNull().SetValidator(new AddressValidator());
        }
    }
}
