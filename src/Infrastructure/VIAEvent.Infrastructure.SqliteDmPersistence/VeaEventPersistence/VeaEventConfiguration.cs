using System.ComponentModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VIAEventAssociation.Core.Domain.Aggregates.Events;
using VIAEventAssociation.Core.Domain.Aggregates.Events.Entities;
using VIAEventAssociation.Core.Domain.Aggregates.Events.Entities.Invitation;
using VIAEventAssociation.Core.Domain.Aggregates.Events.state;
using VIAEventAssociation.Core.Domain.Aggregates.Guests;

namespace VIAEvent.Infrastructure.SqliteDmPersistence.VeaEventPersistence;

public class VeaEventConfiguration : IEntityTypeConfiguration<VeaEvent> {


    // Todo : Ask troels if there is a better alternative that he is aware of..
    private IEventStatusState GetEventStateFromString(string value) {
        EventStatus status = EventStatus.FromString(value);
        if (status.Equals(EventStatus.Draft))
            return DraftStatusState.GetInstance();
        if (status.Equals(EventStatus.Ready))
            return ReadyStatusState.GetInstance();
        if (status.Equals(EventStatus.Active))
            return ActiveStatusState.GetInstance();
        if (status.Equals(EventStatus.Cancelled))
            return CancelledStatusState.GetInstance();

        throw new InvalidEnumArgumentException("Invalid display name");

    }
    public void Configure(EntityTypeBuilder<VeaEvent> entityBuilder) {
        // Id
        entityBuilder.HasKey(x => x.Id);
        entityBuilder
            .Property(x => x.Id)
            .HasConversion(
                mId => mId.Value,
                dbValue => EventId.FromGuid(dbValue));

        // Title
        entityBuilder.ComplexProperty(
            entity => entity.Title,
            propBuilder => {
                propBuilder.Property(vo => vo.Value)
                    .HasColumnName("Title");
            });

        // Description
        entityBuilder.ComplexProperty(
            entity => entity.Description,
            propBuilder => {
                propBuilder.Property(vo => vo.Value)
                    .HasColumnName("Description");
            });

        // MaxGuests
        entityBuilder.ComplexProperty(
            entity => entity.MaxGuests,
            propBuilder => {
                propBuilder.Property(vo => vo.Value)
                    .HasColumnName("MaxGuests");
            });


        // Event Status
        entityBuilder.Property(entity => entity.CurrentStatusState)
            .HasConversion(
                mState => mState.CurrentStatus().DisplayName,
                dbValue => GetEventStateFromString(dbValue));

        // Event Visibility
        entityBuilder.ComplexProperty(entity => entity.Visibility,
            propBuilder => {
                propBuilder.Property(vo => vo.DisplayName)
                    .HasColumnName("Visibility");
            });


        // Duration (nullable , multi-valued)
        entityBuilder.OwnsOne(entity => entity.Duration,
            builder => {
                builder.Property(vo => vo.StartDateTime)
                    .HasColumnName("StartDateTime");

                builder.Property(vo => vo.EndDateTime)
                    .HasColumnName("EndDateTime");
            });

        // Location
        entityBuilder.HasOne(entity => entity.Location)
            .WithMany();

        // Event - Invitations
        entityBuilder.OwnsMany(entity => entity.EventInvitations,
            invitationBuilder => {
                // Id
                invitationBuilder.HasKey(vo => vo.Id);
                invitationBuilder.Property(x => x.Id)
                    .HasConversion(
                        mId => mId.Value,
                        dbValue => EventInvitationId.FromGuid(dbValue));

                // Join Status
                invitationBuilder.Property(vo => vo.Status)
                    .HasConversion(
                        mJoinStatus => mJoinStatus.DisplayName,
                        dbValue =>
                            JoinStatus.FromString(dbValue)
                    );

                // Guest Id
                invitationBuilder.HasOne<Guest>()
                    .WithMany()
                    .HasForeignKey(vo => vo.GuestId);
                
                invitationBuilder.Property(vo => vo.GuestId)
                    .HasConversion(
                        mId => mId.Value,
                        dbValue => GuestId.FromGuid(dbValue)
                    );
            });
    }
}