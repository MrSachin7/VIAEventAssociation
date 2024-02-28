using VIAEventAssociation.Core.Domain.Common.Bases;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Domain.Aggregates.Events;

internal class EventVisibility : Enumeration {


    internal static EventVisibility InVisible = new EventVisibility(0, "InVisible");
    internal static EventVisibility Visible = new EventVisibility(1, "Visible");


    private EventVisibility(){}

    private EventVisibility(int value, string displayName): base(value, displayName){}
    
}