using System;
using Microsoft.Extensions.DependencyInjection;

namespace ParametersManagement.LibParameters.DependencyInjection;

public static class ParametersManagerServicesExtensions
{
    // ReSharper disable once UnusedMethodReturnValue.Global
    public static IServiceCollection AddMainParametersManager(this IServiceCollection services,
        Action<MainParametersManagerOptions> setupAction)
    {
        services.AddSingleton<IParametersManager, ParametersManager>();
        services.Configure(setupAction);
        return services;
    }
}
