namespace ViaEventAssociation.Core.Tools.OperationResult;

public interface IMapper {
    TOutput Map<TOutput> (object input) where TOutput : class;
}