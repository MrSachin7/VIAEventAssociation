using VIAEventAssociation.Core.Domain.Common.Bases;
using VIAEventAssociation.Core.Domain.Common.Repository;

namespace UnitTests.Fakes;

public class TestRepo<T, TId> : IRepository<T, TId> where TId : Id where T : Aggregate<TId> {
    public List<T> Values { get; } = new();

    public Task<T?> FindAsync(TId id) {
        return Task.FromResult(Values.FirstOrDefault(x => x.Id.Equals(id)));
    }

    public Task AddAsync(T aggregate) {
        Values.Add(aggregate);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(T aggregate) {
        Values.Remove(aggregate);
        return Task.CompletedTask;
    }
}