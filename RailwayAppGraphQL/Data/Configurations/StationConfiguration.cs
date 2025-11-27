using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RailwayAppGraphQL.Models;

namespace RailwayAppGraphQL.Data.Configurations;

public class StationConfiguration : IEntityTypeConfiguration<Station>
{
    public void Configure(EntityTypeBuilder<Station> builder)
    {
        // Primary Key
        builder.HasKey(s => s.Id);

        // Properties
        builder.Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(s => s.Address)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(s => s.HasWc)
            .IsRequired();

        builder.Property(s => s.HasCoffeeMachine)
            .IsRequired();

        builder.Property(s => s.HasWaitingRoom)
            .IsRequired();
    }
}