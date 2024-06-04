namespace ViaEventAssociation.Core.Tools.OperationResult;

public interface IMappingConfig<in TInput, out TOutput>
    where TInput : class
    where TOutput : class {

    public TOutput Map (TInput input);
}