using System.Text.Json;

namespace ViaEventAssociation.Core.Tools.OperationResult;

public class ObjectMapper : IMapper{

    private readonly IServiceProvider _serviceProvider;

    public ObjectMapper(IServiceProvider serviceProvider) {
        _serviceProvider = serviceProvider;
    }


    public TOutput Map<TOutput>(object input) 
        where TOutput : class {
        Type type = typeof(IMappingConfig<,>)
            .MakeGenericType(input.GetType(), typeof(TOutput));

        dynamic? mappingConfig = _serviceProvider.GetService(type);

        if (mappingConfig is not null) {
            return mappingConfig.Map((dynamic)input);
        }

        string json = JsonSerializer.Serialize(input);
        return JsonSerializer.Deserialize<TOutput>(json)
               ?? throw new InvalidOperationException(
                   $"Failed to map object of type {input.GetType()} to {typeof(TOutput)}");

    }
}