﻿using VIAEventAssociation.Core.Domain.Common.Bases;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Domain.Aggregates.Locations;

public class LocationMaxGuests : ValueObject{
    internal int Value { get; set; }

    private LocationMaxGuests(int maxGuests) {
        Value = maxGuests;
    }

    private LocationMaxGuests() {
        // Efc needs this
    }

    public static Result<LocationMaxGuests> Create(int value) {
        return Result.ToBuilder(ErrorCode.BadRequest)
            .AssertWithError(() => NoLessThan5(value), ErrorMessage.MaxGuestsNotLessThan5)
            .AssertWithError(() => NoMoreThan50(value), ErrorMessage.MaxGuestsNotMoreThan50)
            .WithPayload(new LocationMaxGuests(value))
            .Build();
    }

    private static bool NoLessThan5(int maxGuests) {
        return maxGuests >= 5;
    }

    private static bool NoMoreThan50(int maxGuests) {
        return maxGuests <= 50;
    }

    protected override IEnumerable<object> GetEqualityComponents() {
        yield return Value;
    }
}