using Microsoft.EntityFrameworkCore;
using VIAEventAssociation.Core.Domain.Contracts;

namespace VIAEvent.Infrastructure.SqliteDmPersistence.Contracts;

public class UniqueEmailChecker : IUniqueEmailChecker {
    private readonly SqliteWriteDbContext _context;

    public UniqueEmailChecker(SqliteWriteDbContext context) {
        _context = context;
    }

    public async Task<bool> IsUnique(string email) {
        bool doesEmailExists =
            await _context.Guests.AnyAsync(guest => guest.Email.Value.ToLower().Equals(email.ToLower()));
        return !doesEmailExists;
    }
}