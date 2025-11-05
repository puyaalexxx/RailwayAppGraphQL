using Microsoft.EntityFrameworkCore;
using RailwayAppGraphQL.Models;
using RailwayAppGraphQL.Models.Tickets;
using RailwayAppGraphQL.Models.Trains;

namespace RailwayAppGraphQL.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Train> Trains => Set<Train>();
    public DbSet<Station> Stations => Set<Station>();
    public DbSet<Stop> Stops => Set<Stop>();
    public DbSet<Ticket> Tickets => Set<Ticket>();
}