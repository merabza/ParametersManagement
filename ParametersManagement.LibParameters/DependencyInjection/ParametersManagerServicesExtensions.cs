using System;
using Microsoft.Extensions.DependencyInjection;

namespace ParametersManagement.LibParameters.DependencyInjection;

public static class ParametersManagerServicesExtensions
{
    // ReSharper disable once UnusedMethodReturnValue.Global
    public static IServiceCollection AddMainParametersManager<T>(this IServiceCollection services,
        Action<MainParametersManagerOptions> setupAction) where T : ParametersManager
    {
        services.AddSingleton<IParametersManager, T>();
        services.Configure(setupAction);
        return services;
    }
}
