using AzureParcelTracking.Commands;
using FluentValidation;

namespace AzureParcelTracking.Application.Validators
{
    public class AddTrackingCommandValidator : AbstractValidator<AddTrackingCommand>
    {
        public AddTrackingCommandValidator()
        {
            RuleFor(cmd => cmd.CreatedByUserId).NotEmpty();
            RuleFor(cmd => cmd.Tracking).NotNull().SetValidator(new TrackingValidator());
        }
    }
}