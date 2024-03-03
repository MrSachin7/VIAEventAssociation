using ViaEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Domain.Aggregates.Events.Entities;

public class JoinStatus : Enumeration {
    

    internal static readonly JoinStatus Pending = new JoinStatus(0, "Pending");
    internal static readonly JoinStatus Accepted = new JoinStatus(1, "Accepted");
    internal static readonly JoinStatus Declined = new JoinStatus(2, "Declined");

    private JoinStatus(){}
    private JoinStatus(int value, string displayName): base(value, displayName){}

    
}