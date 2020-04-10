using AzureParcelTracking.Commands.Models;
using FluentValidation;

namespace AzureParcelTracking.Application.Validators
{
    internal class AddressValidator : AbstractValidator<Address>
    {
        public AddressValidator()
        {
            RuleFor(address => address.Address1).NotEmpty();
            RuleFor(address => address.Name).NotEmpty();
            RuleFor(address => address.Phone).NotEmpty();
            RuleFor(address => address.Postcode).NotEmpty();
        }
    }
}
