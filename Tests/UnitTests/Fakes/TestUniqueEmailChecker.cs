using VIAEventAssociation.Core.Domain.temp;

namespace UnitTests.Fakes;

internal class TestUniqueEmailChecker : IUniqueEmailChecker {

    internal bool InitialValue { get; set; } = true;

    public Task<bool> IsUnique(string email) {
        return Task.FromResult(InitialValue);
    }
}