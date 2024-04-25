using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VIAEventAssociation.Core.Domain.Aggregates.Locations;

namespace VIAEvent.Infrastructure.SqliteDmPersistence.Configuration;

public class LocationConfiguration : IEntityTypeConfiguration<Location> {

    public void Configure(EntityTypeBuilder<Location> builder) {
        builder.HasKey(location => location.Id);
        builder.Property(location => location.Id)
            .HasConversion(
                vo => vo.Value,
                dbValue => LocationId.FromGuid(dbValue));

        builder.ComplexProperty(location => location.LocationName,
            propBuilder => {
                propBuilder.Property(locationName => locationName.Value)
                    .HasColumnName("LocationName");
            });

        builder.ComplexProperty(location => location.LocationMaxGuests,
            propBuilder => {
                propBuilder.Property(locationMaxGuests => locationMaxGuests.Value)
                    .HasColumnName("LocationMaxGuests");
            });

    }
}