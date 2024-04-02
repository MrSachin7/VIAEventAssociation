using VIAEventAssociation.Core.Domain.Contracts;

namespace UnitTests.Fakes;

internal class TestUniqueEmailChecker : IUniqueEmailChecker {

    internal bool Value { get; set; } = true;

    public Task<bool> IsUnique(string email) {
        return Task.FromResult(Value);
    }
}