using VIAEventAssociation.Core.QueryContracts.Contracts;

namespace VIAEventAssociation.Core.QueryContracts.Queries;

public static class GuestsRankingQuery {

    public record Query(
        int PageNumber,
        int GuestsPerPage
        ) : IQuery<Answer>;

    public record Answer(ICollection<Guest> Guests,
        int TotalPages);


    public record Guest(
        string GuestId,
        string FullName,
        string Email,
        int ParticipationCount);

}