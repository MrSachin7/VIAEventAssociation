using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VIAEventAssociation.Core.Domain.Aggregates.Events;
using VIAEventAssociation.Core.Domain.Aggregates.Guests;

namespace VIAEvent.Infrastructure.SqliteDmPersistence.VeaEventPersistence;

public class EventParticipationConfiguration : IEntityTypeConfiguration<EventParticipation> {


    public void Configure(EntityTypeBuilder<EventParticipation> builder) {
        builder.HasKey(o => new{o.EventId, o.GuestId});
        builder.HasOne<VeaEvent>()
            .WithMany(o => o.IntendedParticipants)
            .HasForeignKey(o => o.EventId);

        builder.Property(m => m.EventId)
            .HasConversion(
                mId => mId.Value,
                dbValue => EventId.FromGuid(dbValue));

        builder.Property(m => m.GuestId)
            .HasConversion(
                mId => mId.Value,
                dbValue => GuestId.FromGuid(dbValue));

        builder.HasOne<Guest>()
            .WithMany()
            .HasForeignKey(y => y.GuestId);
    }
}