using ViaEventAssociation.Core.Tools.OperationResult;

namespace IntegrationTests.Query;

public class QueryTestSystemTime : ISystemTime{
    public DateTime CurrentTime() {
        // Since the fake data has events ranging from 2024-01-01 to 2024-04-30,
        // we can set the current time to 2024-03-28 08:00:00, so that we include all scenarios in the query
        return new DateTime(2024, 3, 28, 8, 0 ,0);
    }
}