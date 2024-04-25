using System.ComponentModel;
using System.Reflection;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Domain.Aggregates.Events;

public class EventVisibility : Enumeration {


    internal static readonly EventVisibility Private = new EventVisibility(0, "Private");
    internal static readonly EventVisibility Public = new EventVisibility(1, "Public");


    private EventVisibility(){}

    private EventVisibility(int value, string displayName): base(value, displayName){}
    

    public static EventVisibility FromString(string displayName) {
        return GetAll().FirstOrDefault(e => e.DisplayName.Equals(displayName, StringComparison.OrdinalIgnoreCase)) ??
               throw new InvalidEnumArgumentException("Invalid display name");
    }

    private static IEnumerable<EventVisibility> GetAll() {
        var fields =
            typeof(EventVisibility).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

        foreach (var info in fields) {
            var instance = new EventVisibility();
            var locatedValue = info.GetValue(instance) as EventVisibility;

            if (locatedValue != null) {
                yield return locatedValue;
            }
        }
    }
}