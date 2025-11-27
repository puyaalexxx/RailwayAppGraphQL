using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RailwayAppGraphQL.Models;

namespace RailwayAppGraphQL.Data.Configurations;

public class StopConfiguration : IEntityTypeConfiguration<Stop>
{
    public void Configure(EntityTypeBuilder<Stop> builder)
    {
        // Primary Key
        builder.HasKey(t => t.Id);

        builder.Property(s => s.DepartureTimeUtc)
            .IsRequired();

        builder.Property(s => s.ArrivalTimeUtc)
            .IsRequired();

        // Stop → Train (many-to-one)
        builder.HasOne(s => s.Train)
            .WithMany(t => t.Stops)
            .HasForeignKey(s => s.TrainId)
            .OnDelete(DeleteBehavior.Cascade); // Deleting a Train deletes its Stops.

        // Stop → Station (many-to-one)
        builder.HasOne(s => s.Station)
            .WithMany(st => st.Stops)
            .HasForeignKey(s => s.StationId)
            .OnDelete(DeleteBehavior.Restrict); // Cannot delete a Station if a Stop points to it.
    }
}