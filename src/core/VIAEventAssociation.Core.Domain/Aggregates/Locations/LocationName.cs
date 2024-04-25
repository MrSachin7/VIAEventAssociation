using System.Text.RegularExpressions;
using VIAEventAssociation.Core.Domain.Common.Bases;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Domain.Aggregates.Locations;

public class LocationName : ValueObject{
    public string Value { get; set; }


    private LocationName(string name) {
        Value = name;
    }

    private LocationName() {
        // Efc needs this
    }

    public static Result<LocationName> Create(string name) {
        return Result.ToBuilder(ErrorCode.BadRequest)
            .AssertWithError(() => ValidLocationName(name), ErrorMessage.InvalidLocationName)
            .WithPayload(new LocationName(name))
            .Build();

    }

    private static bool ValidLocationName(string name) {
        if (string.IsNullOrEmpty(name)) {
            return false;
        }

        const string pattern = @"^[ABC][0][0-5]\.[0-9][0-9]$";
        return Regex.IsMatch(name, pattern);

    }


    protected override IEnumerable<object> GetEqualityComponents() {
        yield return Value;
    }
}