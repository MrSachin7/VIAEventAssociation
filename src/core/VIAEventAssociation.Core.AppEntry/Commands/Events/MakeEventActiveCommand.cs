﻿using VIAEventAssociation.Core.Domain.Aggregates.Events;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.AppEntry.Commands.Events;

public class MakeEventActiveCommand {
    public EventId Id { get; init; }

    private MakeEventActiveCommand(EventId id) {
        Id = id;
    }

    public static Result<MakeEventActiveCommand> Create(string eventId) {
        Result<EventId> idResult = EventId.Create(eventId);
        return idResult.WithPayload(() => new MakeEventActiveCommand(idResult.Payload!));
    }
}