using AzureParcelTracking.Commands.Models;
using FluentValidation;

namespace AzureParcelTracking.Application.Validators
{
    public class TrackingValidator : AbstractValidator<NewTracking>
    {
        public TrackingValidator()
        {
            RuleFor(tracking => tracking.ConsignmentId).NotNull();
            RuleFor(tracking => tracking.CurrentAddress).NotNull().SetValidator(new TrackingAddressValidator());
        }
    }
}