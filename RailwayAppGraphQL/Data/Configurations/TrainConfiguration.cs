using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RailwayAppGraphQL.Models.Trains;

namespace RailwayAppGraphQL.Data.Configurations;

public class TrainConfiguration : IEntityTypeConfiguration<Train>
{
    public void Configure(EntityTypeBuilder<Train> builder)
    {
        // Configure PK property first
        builder.Property(t => t.Id)
            .HasColumnType("varchar(255)")
            .IsRequired();

        // Primary Key
        builder.HasKey(t => t.Id);

        // Properties
        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(t => t.Number)
            .IsRequired()
            .HasMaxLength(20);

        /*modelBuilder.Entity<Stop>()
            .HasOne(s => s.Train)
            .WithMany(t => t.Stops)
            .HasForeignKey(s => s.TrainId)
            .OnDelete(DeleteBehavior.Cascade); // deleting train deletes stops

        modelBuilder.Entity<Ticket>()
            .HasOne(t => t.Train)
            .WithMany(tr => tr.Tickets)
            .HasForeignKey(t => t.TrainId)
            .OnDelete(DeleteBehavior.Cascade); // deleting train deletes tickets

        modelBuilder.Entity<Stop>()
            .HasOne(s => s.Station)
            .WithMany(st => st.Stops)
            .HasForeignKey(s => s.StationId)
            .OnDelete(DeleteBehavior.Restrict); // cannot delete station if stops exist*/
    }
}