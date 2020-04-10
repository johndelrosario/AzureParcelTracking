using AzureParcelTracking.Commands.Models;
using FluentValidation;

namespace AzureParcelTracking.Application.Validators
{
    internal class TrackingAddressValidator : AbstractValidator<Address>
    {
        public TrackingAddressValidator()
        {
            RuleFor(address => address.Address1).NotEmpty();
            RuleFor(address => address.Postcode).NotEmpty();
        }
    }
}