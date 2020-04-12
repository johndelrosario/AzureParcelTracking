using AutoMapper;
using AzureFromTheTrenches.Commanding.Abstractions;
using AzureParcelTracking.Application.Repositories.Implementation;
using AzureParcelTracking.Application.Repositories.Interfaces;
using AzureParcelTracking.Application.Validators;
using AzureParcelTracking.Commands;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace AzureParcelTracking.Application
{
    public static class SubsystemRegistration
    {
        public static IServiceCollection AddApplication(this IServiceCollection services,
            ICommandRegistry commandRegistry)
        {
            services
                .AddAutoMapper(typeof(SubsystemRegistration))
                .AddValidators()
                .AddRepositories();

            commandRegistry.Discover(typeof(SubsystemRegistration).Assembly);

            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            return services
                .AddTransient<IConsignmentRepository, ConsignmentRepository>()
                .AddTransient<ITrackingRepository, TrackingRepository>();
        }

        private static IServiceCollection AddValidators(this IServiceCollection services)
        {
            return services
                .AddTransient<IValidator<AddConsignmentCommand>, AddConsignmentCommandValidator>()
                .AddTransient<IValidator<GetConsignmentQuery>, GetConsignmentQueryValidator>()
                .AddTransient<IValidator<AddTrackingCommand>, AddTrackingCommandValidator>();
        }
    }
}