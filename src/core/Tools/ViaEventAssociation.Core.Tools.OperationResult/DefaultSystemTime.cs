namespace ViaEventAssociation.Core.Tools.OperationResult;

public class DefaultSystemTime : ISystemTime {
    public DateTime CurrentTime() {
        return DateTime.Now;
    }
}