using ViaEventAssociation.Core.Tools.OperationResult;

namespace UnitTests.Common.Stubs;

public class TestSystemTime : ISystemTime{

    // Test current time would be 2024 January 1st 00:00:00
    public DateTime CurrentTime() {
        return new DateTime(2024, 1, 1, 0, 0, 0);
    }
}