using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VIAEventAssociation.Core.Domain.Aggregates.Guests;

namespace VIAEvent.Infrastructure.SqliteDmPersistence.Configuration;

public class GuestConfiguration : IEntityTypeConfiguration<Guest> {
    public void Configure(EntityTypeBuilder<Guest> builder) {
        // GuestId
        builder.HasKey(guest => guest.Id);
        builder.Property(guest => guest.Id)
            .HasConversion(
                vo => vo.Value,
                dbValue => GuestId.FromGuid(dbValue)
            );

        // FirstName
        builder.ComplexProperty(guest => guest.FirstName,
            propBuilder => {
                propBuilder.Property(vo => vo.Value)
                    .HasColumnName("FirstName");
            });

        // LastName
        builder.ComplexProperty(guest => guest.LastName,
            propBuilder => {
                propBuilder.Property(vo => vo.Value)
                    .HasColumnName("LastName");
            });

        // Email
        builder.ComplexProperty(guest => guest.Email,
            propBuilder => {
                propBuilder.Property(vo => vo.Value)
                    .HasColumnName("Email");
            });

        // ProfilePictureUrl
        builder.OwnsOne(guest => guest.ProfilePictureUrl,
            propBuilder => {
                propBuilder.Property(vo => vo.Url)
                    .HasColumnName("ProfilePictureUrl");
            });
    }
}