using System.ComponentModel;
using System.Reflection;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Domain.Aggregates.Events.Entities;

public class JoinStatus : Enumeration {
    

    public static readonly JoinStatus Pending = new JoinStatus(0, "Pending");
    public static readonly JoinStatus Accepted = new JoinStatus(1, "Accepted");
    public static readonly JoinStatus Declined = new JoinStatus(2, "Declined");

    private JoinStatus(){}
    private JoinStatus(int value, string displayName): base(value, displayName){}


    public static JoinStatus FromString(string displayName) {
        return GetAll().FirstOrDefault(e => e.DisplayName.Equals(displayName, StringComparison.OrdinalIgnoreCase)) ??
               throw new InvalidEnumArgumentException("Invalid display name");
    }

    private static IEnumerable<JoinStatus> GetAll() {
        var fields =
            typeof(JoinStatus).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

        foreach (var info in fields) {
            var instance = new JoinStatus();
            var locatedValue = info.GetValue(instance) as JoinStatus;

            if (locatedValue != null) {
                yield return locatedValue;
            }
        }
    }
    
}