//using System;
//using System.Linq;
//using System.Reflection;
//using Microsoft.Extensions.DependencyInjection;

//namespace ParametersManagement.LibParameters.DependencyInjection;

//public static class ToolCommandCollectionExtensions
//{
//    public static void AddTransientAllToolCommandFactoryStrategies(this IServiceCollection services,
//        params Assembly[] assemblies)
//    {
//        foreach (Assembly assembly in assemblies)
//        {
//            foreach (Type type in assembly.ExportedTypes.Where(x =>
//                         typeof(IToolCommandFactoryStrategy).IsAssignableFrom(x) &&
//                         x is { IsInterface: false, IsAbstract: false }))
//            {
//                services.AddTransient(typeof(IToolCommandFactoryStrategy), type);
//            }
//        }
//    }
//}
