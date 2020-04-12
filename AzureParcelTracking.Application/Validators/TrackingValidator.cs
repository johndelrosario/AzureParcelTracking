using AzureParcelTracking.Commands.Models;
using FluentValidation;

namespace AzureParcelTracking.Application.Validators
{
    public class TrackingValidator : AbstractValidator<NewTracking>
    {
        public TrackingValidator()
        {
            RuleFor(tracking => tracking.ConsignmentId).NotEmpty();
            RuleFor(tracking => tracking.Address).NotNull().SetValidator(new TrackingAddressValidator());
            RuleFor(tracking => tracking.TrackingType).NotEmpty();
        }
    }
}