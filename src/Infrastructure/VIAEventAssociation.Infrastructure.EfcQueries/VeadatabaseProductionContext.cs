
using Microsoft.EntityFrameworkCore;

namespace VIAEventAssociation.Infrastructure.EfcQueries;

public partial class VeadatabaseProductionContext : DbContext
{
    public VeadatabaseProductionContext()
    {
    }

    public VeadatabaseProductionContext(DbContextOptions<VeadatabaseProductionContext> options)
        : base(options)
    {
    }

    public virtual DbSet<EventInvitation> EventInvitations { get; set; }

    public virtual DbSet<Guest> Guests { get; set; }

    public virtual DbSet<Location> Locations { get; set; }

    public virtual DbSet<VeaEvent> VeaEvents { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EventInvitation>(entity =>
        {
            entity.ToTable("EventInvitation");

            entity.HasIndex(e => e.GuestId, "IX_EventInvitation_GuestId");

            entity.HasIndex(e => e.VeaEventId, "IX_EventInvitation_VeaEventId");

            entity.HasOne(d => d.Guest).WithMany(p => p.EventInvitations).HasForeignKey(d => d.GuestId);

            entity.HasOne(d => d.VeaEvent).WithMany(p => p.EventInvitations).HasForeignKey(d => d.VeaEventId);
        });

        modelBuilder.Entity<VeaEvent>(entity =>
        {
            entity.HasIndex(e => e.LocationId, "IX_VeaEvents_LocationId");

            entity.Property(e => e.EndDateTime).HasColumnType("DATETIME");
            entity.Property(e => e.StartDateTime).HasColumnType("DATETIME");

            entity.HasOne(d => d.Location).WithMany(p => p.VeaEvents).HasForeignKey(d => d.LocationId);

            entity.HasMany(d => d.Guests).WithMany(p => p.Events)
                .UsingEntity<Dictionary<string, object>>(
                    "EventParticipation",
                    r => r.HasOne<Guest>().WithMany().HasForeignKey("GuestId"),
                    l => l.HasOne<VeaEvent>().WithMany().HasForeignKey("EventId"),
                    j =>
                    {
                        j.HasKey("EventId", "GuestId");
                        j.ToTable("EventParticipation");
                        j.HasIndex(new[] { "GuestId" }, "IX_EventParticipation_GuestId");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
