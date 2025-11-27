using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RailwayAppGraphQL.Models.Trains;

namespace RailwayAppGraphQL.Data.Configurations;

public class TrainConfiguration : IEntityTypeConfiguration<Train>
{
    public void Configure(EntityTypeBuilder<Train> builder)
    {
        // Primary Key
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(t => t.Number)
            .IsRequired()
            .HasMaxLength(15);

        builder.Property(t => t.Type)
            .IsRequired()
            .HasConversion<string>() // store enum as string
            .HasMaxLength(20);

        builder.Property(t => t.Status)
            .IsRequired()
            .HasConversion<string>() // store enum as string
            .HasMaxLength(20);

        builder.Property(t => t.Seats)
            .IsRequired();
    }
}