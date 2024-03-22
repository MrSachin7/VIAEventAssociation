using VIAEventAssociation.Core.Domain.Aggregates.Events;

namespace UnitTests.Fakes;

public class TestEventRepo : TestRepo<VeaEvent, EventId>, IEventRepository {

    public static TestEventRepo With(VeaEvent veaEvent) {
        TestEventRepo repo = new();
        repo.Values.Add(veaEvent);
        return repo;
    }

}