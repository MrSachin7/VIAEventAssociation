namespace VIAEventAssociation.Core.Domain.Common.Bases;

public abstract class ValueObject {
    public override bool Equals(object? other) {
        if (other is null) return false;
        if (other.GetType() != GetType()) return false;

        return ((ValueObject)other).GetEqualityComponents()
            .SequenceEqual(GetEqualityComponents());
    }

    protected abstract IEnumerable<object> GetEqualityComponents();

    public override int GetHashCode() {
        return GetEqualityComponents()
            .Select(obj => obj?.GetHashCode() ?? 0)
            .Aggregate((x, y) => x ^ y);
    }
}