using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using VIAEventAssociation.Core.QueryContracts.Contracts;

namespace VIAEventAssociation.Infrastructure.EfcQueries;

public static class QueryCollectionExtension {

    public static IServiceCollection RegisterQueryHandlers(this IServiceCollection services) {
        Type handlerType = typeof(IQueryHandler<,>);

        // Gets the current assembly
        Assembly currentAssembly = Assembly.GetExecutingAssembly();

        List<Type> allHandlerTypes = currentAssembly.GetTypes()
            .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == handlerType))
            .ToList();

        foreach (Type type in allHandlerTypes) {
            Type[] interfaces = type.GetInterfaces();
            Type queryType = interfaces.First(i => i.IsGenericType && i.GetGenericTypeDefinition() == handlerType);
            services.AddScoped(queryType, type);

            Console.WriteLine($"Successfully registered query handler : {type.Name} with type {queryType.Name}");
        }

        return services;
    }
}