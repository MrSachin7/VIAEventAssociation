using Microsoft.EntityFrameworkCore;
using VIAEvent.Infrastructure.SqliteDmPersistence;
using VIAEventAssociation.Core.Domain.Aggregates.Events;
using VIAEventAssociation.Core.Domain.Aggregates.Events.Entities;
using VIAEventAssociation.Core.Domain.Aggregates.Events.Entities.Invitation;
using VIAEventAssociation.Core.Domain.Aggregates.Events.state;
using VIAEventAssociation.Core.Domain.Aggregates.Guests;
using VIAEventAssociation.Core.Domain.Contracts;

namespace IntegrationTests.DmConfiguration;

public class VeaEventDmConfigTests : DmConfigTestHelper {
    [Fact]
    public async Task StrongIdAsPk() {
        await using SqliteWriteDbContext context = SetupContext();

        // This automatically sets a new strong id as pk
        VeaEvent veaEvent = VeaEvent.Empty();

        await SaveAndClearAsync(veaEvent, context);


        VeaEvent? retrieved = context.Events.SingleOrDefault(x => x.Id.Equals(veaEvent.Id));
        Assert.NotNull(retrieved);
        Assert.Equal(veaEvent.Id, retrieved.Id);
    }

    [Fact]
    public async Task NonNullableSinglePrimitiveValuedValueObject() {
        await using SqliteWriteDbContext context = SetupContext();
        EventTitle title = EventTitle.Create("Hello World").Payload!;
        VeaEvent veaEvent = VeaEvent.Empty();

        veaEvent.UpdateEventTitle(title);
        await SaveAndClearAsync(veaEvent, context);

        VeaEvent? retrieved = context.Events.SingleOrDefault(x => x.Id.Equals(veaEvent.Id));
        Assert.NotNull(retrieved);
        Assert.Equal(veaEvent.Id, retrieved.Id);
        Assert.Equal(veaEvent.Title, retrieved.Title);
    }

    [Fact]
    public async Task NullableSinglePrimitiveValuedValueObject() {
        await using SqliteWriteDbContext context = SetupContext();
        VeaEvent veaEvent = VeaEvent.Empty();

        await SaveAndClearAsync(veaEvent, context);

        VeaEvent? retrieved = context.Events.SingleOrDefault(x => x.Id.Equals(veaEvent.Id));
        Assert.NotNull(retrieved);
        Assert.Null(veaEvent.Duration);
    }

    [Fact]
    public async Task ListOfStrongIdFkReferences() {
        await using SqliteWriteDbContext context = SetupContext();
        Guest guest1 = Guest.Create(GuestFirstName.Create("Sachin").Payload!, GuestLastName.Create("Baral").Payload!,
            (await ViaEmail.Create("310628@via.dk", new UniqueEmailChecker())).Payload!).Payload!;

        Guest guest2 = Guest.Create(GuestFirstName.Create("Troels").Payload!,
            GuestLastName.Create("Mortensen").Payload!,
            (await ViaEmail.Create("TRMO@via.dk", new UniqueEmailChecker())).Payload!).Payload!;

        await context.Guests.AddRangeAsync(guest1, guest2);
        await context.SaveChangesAsync();
        context.ChangeTracker.Clear();

        VeaEvent veaEvent1 = VeaEvent.Empty();
        veaEvent1.IntendedParticipants.Add(guest1.Id);
        veaEvent1.IntendedParticipants.Add(guest2.Id);

        await SaveAndClearAsync(veaEvent1, context);

        VeaEvent retrieved = context.Events.Include(ev => ev.IntendedParticipants)
            .Single(x => x.Id.Equals(veaEvent1.Id));

        Assert.NotEmpty(retrieved.IntendedParticipants);
        Assert.Contains(retrieved.IntendedParticipants, x => x.GuestId.Equals(guest1.Id));
        Assert.Contains(retrieved.IntendedParticipants, x => x.GuestId.Equals(guest2.Id));
        Assert.Contains(retrieved.IntendedParticipants, x => x.EventId.Equals(veaEvent1.Id));
    }

    [Fact]
    public async Task ClassAsEnum() {
        await using SqliteWriteDbContext context = SetupContext();
        VeaEvent veaEvent = VeaEvent.Empty();

        await SaveAndClearAsync(veaEvent, context);

        VeaEvent retrieved = context.Events.Single(x => x.Id.Equals(veaEvent.Id));

        Assert.Equal(EventStatus.Draft, retrieved.Status);
    }

    [Fact]
    public async Task ListOfEntities() {
        await using SqliteWriteDbContext context = SetupContext();
        VeaEvent veaEvent = VeaEvent.Empty();

        Guest guest1 = Guest.Create(GuestFirstName.Create("Sachin").Payload!, GuestLastName.Create("Baral").Payload!,
            (await ViaEmail.Create("310628@via.dk", new UniqueEmailChecker())).Payload!).Payload!;
        Guest guest2 = Guest.Create(GuestFirstName.Create("Troels").Payload!,
            GuestLastName.Create("Mortensen").Payload!,
            (await ViaEmail.Create("TRMO@via.dk", new UniqueEmailChecker())).Payload!).Payload!;

        EventInvitation invitation1 = EventInvitation.Create(guest1.Id);
        EventInvitation invitation2 = EventInvitation.Create(guest2.Id);

        await context.Guests.AddRangeAsync(guest1, guest2);
        await context.Events.AddAsync(veaEvent);
        await context.SaveChangesAsync();

        VeaEvent retrieved = context.Events.Include(ev => ev.EventInvitations).Single(x => x.Id.Equals(veaEvent.Id));
        retrieved.CurrentStatusState = ReadyStatusState.GetInstance();
        retrieved.InviteGuest(invitation1);
        retrieved.InviteGuest(invitation2);

        await context.SaveChangesAsync();

        // Retrieve the event again
        retrieved = context.Events.Include(ev => ev.EventInvitations).Single(x => x.Id.Equals(veaEvent.Id));

        Assert.NotEmpty(retrieved.EventInvitations);
        Assert.Contains(retrieved.EventInvitations, x => x.GuestId.Equals(guest1.Id));
        Assert.Contains(retrieved.EventInvitations, x => x.GuestId.Equals(guest2.Id));
        Assert.Contains(retrieved.EventInvitations, x => x.Status.Equals(JoinStatus.Pending));


    }


    private class UniqueEmailChecker : IUniqueEmailChecker {
        public Task<bool> IsUnique(string email) {
            return Task.FromResult(true);
        }
    };


    // Rainy cannot be tested because the domain doesnt allow it..
}