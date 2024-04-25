using System.ComponentModel;
using System.Reflection;
using VIAEventAssociation.Core.Domain.Common.Bases;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Domain.Aggregates.Events;

public class EventStatus : Enumeration {
    internal static readonly EventStatus Draft
        = new EventStatus(0, "Draft");

    internal static readonly EventStatus Ready
        = new EventStatus(1, "Ready");

    internal static readonly EventStatus Active
        = new EventStatus(2, "Active");

    internal static readonly EventStatus Cancelled
        = new EventStatus(3, "Cancelled");




    private EventStatus() {
    }

    private EventStatus(int value, string displayName) : base(value, displayName) {
    }


    public static EventStatus FromString(string displayName) {
        return GetAll().FirstOrDefault(e => e.DisplayName.Equals(displayName, StringComparison.OrdinalIgnoreCase)) ??
               throw new InvalidEnumArgumentException("Invalid display name");
    }

    private static IEnumerable<EventStatus> GetAll() {
        var fields =
            typeof(EventStatus).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.DeclaredOnly );

        foreach (var info in fields) {
            var instance = new EventStatus();
            var locatedValue = info.GetValue(instance) as EventStatus;

            if (locatedValue != null) {
                yield return locatedValue;
            }
        }
    }
}