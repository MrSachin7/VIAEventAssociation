using VIAEventAssociation.Core.Domain.Common.Bases;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Domain.Aggregates.Events;

// TODO: Ask troels if these enums (implemented as Enumerations) are also valueobjects
internal class EventStatus : Enumeration {

    internal static readonly EventStatus Draft
        = new EventStatus(0, "Draft"); 
    internal static readonly EventStatus Ready
        = new EventStatus(1, "Ready");
    internal static readonly EventStatus Active
        = new EventStatus(2, "Active");
    internal static readonly EventStatus Cancelled
        = new EventStatus(3, "Cancelled");




    private EventStatus(){}

    private EventStatus(int value, string displayName) : base(value, displayName){}

}