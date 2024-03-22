using VIAEventAssociation.Core.Domain.Common.UnitOfWork;

namespace UnitTests.Fakes;

public class TestUnitOfWork : IUnitOfWork {
    public Task SaveChangesAsync() {
        return Task.CompletedTask;
    }
}