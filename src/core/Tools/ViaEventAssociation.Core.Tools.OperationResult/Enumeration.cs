using System.ComponentModel;
using System.Reflection;

namespace ViaEventAssociation.Core.Tools.OperationResult;

// This class is copied from :
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