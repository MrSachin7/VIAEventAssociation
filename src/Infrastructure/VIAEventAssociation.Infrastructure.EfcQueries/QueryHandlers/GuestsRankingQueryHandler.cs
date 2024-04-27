using System.Collections.Immutable;
using Microsoft.EntityFrameworkCore;
using VIAEventAssociation.Core.QueryContracts.Contracts;
using VIAEventAssociation.Core.QueryContracts.Queries;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace VIAEventAssociation.Infrastructure.EfcQueries.QueryHandlers;

public class GuestsRankingQueryHandler: IQueryHandler<GuestsRankingQuery.Query, GuestsRankingQuery.Answer> {
    private readonly VeadatabaseProductionContext _context;

    public GuestsRankingQueryHandler(VeadatabaseProductionContext context) {
        _context = context;
    }


    public async Task<Result<GuestsRankingQuery.Answer>> HandleAsync(GuestsRankingQuery.Query query) {
        IQueryable<Guest> allGuests = _context.Guests.AsQueryable();
        int totalGuestsAmount = await allGuests.CountAsync();
        int totalPages = (int) Math.Ceiling((double) totalGuestsAmount / query.GuestsPerPage);



        ImmutableList<GuestsRankingQuery.Guest> guests = (await allGuests.Include(guest => guest.Events)
            .OrderByDescending(guest => guest.Events.Count)
            .Skip(query.GuestsPerPage * (query.PageNumber - 1)).Take(query.GuestsPerPage)
            .Select(guest => new GuestsRankingQuery.Guest(guest.Id,$"{guest.FirstName} {guest.LastName}", guest.Email, guest.Events.Count))
            .ToListAsync()).ToImmutableList();

        return new GuestsRankingQuery.Answer(guests, totalPages);
    }
}