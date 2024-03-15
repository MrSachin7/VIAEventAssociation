using VIAEventAssociation.Core.Domain.Common.Bases;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Core.Domain.Aggregates.Guests;

public class ProfilePictureUrl : ValueObject {
    internal Uri Url { get; }

    private ProfilePictureUrl(Uri url) {
        Url = url;
    }

    internal static Result<ProfilePictureUrl> Create(string url) {
        bool isValid = Uri.TryCreate(url, UriKind.Absolute, out Uri? createdUrl);
        if (!isValid) {
            return Error.BadRequest(ErrorMessage.InvalidUrl);
        }

        return new ProfilePictureUrl(createdUrl!);
    }


    protected override IEnumerable<object> GetEqualityComponents() {
        yield return Url;
    }
}