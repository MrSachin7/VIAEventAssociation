using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using VIAEventAssociation.Core.AppEntry;

namespace VIAEventAssociation.Core.Application;


public static class CommandCollectionExtension {

    public static IServiceCollection RegisterCommandHandlers(this IServiceCollection services) {
        Type handlerType = typeof(ICommandHandler<>);
        // Gets the current assembly
        Assembly currentAssembly = Assembly.GetExecutingAssembly();

        List<Type> allHandlerTypes = currentAssembly.GetTypes()
            .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == handlerType))
            .ToList();

        foreach (Type type in allHandlerTypes) {
            Type[] interfaces = type.GetInterfaces();
            Type commandType = interfaces.First(i => i.IsGenericType && i.GetGenericTypeDefinition() == handlerType);
            services.AddScoped(commandType,type);

            Console.WriteLine($"Successfully registered command handler : {type.Name} with type {commandType.Name}");
        }

        return services;
    }
    
}