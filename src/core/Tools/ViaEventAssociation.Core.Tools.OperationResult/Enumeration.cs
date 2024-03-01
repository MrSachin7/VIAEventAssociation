using System.Reflection;

namespace ViaEventAssociation.Core.Tools.OperationResult;

// This class is copied from :
// https://lostechies.com/jimmybogard/2008/08/12/enumeration-classes/
public abstract class Enumeration 
{

    public int Value { get; }
    public string DisplayName { get; }

    protected Enumeration()
    {
    }

    protected Enumeration(int value, string displayName)
    {
        Value = value;
        DisplayName = displayName;
    }

    public override string ToString()
    {
        return DisplayName;
    }

    public static IEnumerable<T> GetAll<T>() where T : Enumeration, new()
    {
        var type = typeof(T);
        var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

        foreach (var info in fields)
        {
            var instance = new T();
            var locatedValue = info.GetValue(instance) as T;

            if (locatedValue != null)
            {
                yield return locatedValue;
            }
        }
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Enumeration otherValue)
        {
            return false;
        }

        var typeMatches = GetType().Equals(obj.GetType());
        var valueMatches = Value.Equals(otherValue.Value);
        var displayNameMatches = DisplayName.Equals(otherValue.DisplayName);

        return typeMatches && valueMatches && displayNameMatches;
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode() * DisplayName.GetHashCode();
    }

}