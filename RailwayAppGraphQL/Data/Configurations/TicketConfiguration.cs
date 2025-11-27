//noinspection EntityFramework.ModelValidation.UnlimitedStringLength

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RailwayAppGraphQL.Models.Tickets;

namespace RailwayAppGraphQL.Data.Configurations;

public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
{
    public void Configure(EntityTypeBuilder<Ticket> builder)
    {
        // Primary Key
        builder.HasKey(t => t.Id);

        // Properties
        builder.Property(t => t.Number)
            .IsRequired()
            .HasMaxLength(25);

        builder.Property(t => t.PassengerName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(t => t.PassengerEmail)
            .HasMaxLength(50);

        builder.Property(t => t.SeatNumber)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(t => t.Price)
            .IsRequired();

        builder.Property(t => t.Currency)
            .IsRequired();

        builder.Property(t => t.PurchasedAtUtc)
            .IsRequired();

        // Ticket → Train (many-to-one)
        builder.HasOne(t => t.Train)
            .WithMany(tr => tr.Tickets)
            .HasForeignKey(t => t.TrainId)
            .OnDelete(DeleteBehavior.Cascade); // Deleting a Train deletes its Tickets

        // add an index on TrainId for performance
        builder.HasIndex(t => t.TrainId);
    }
}