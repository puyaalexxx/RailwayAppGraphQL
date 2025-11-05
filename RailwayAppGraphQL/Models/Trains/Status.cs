namespace RailwayAppGraphQL.Models.Trains;

public enum Status
{
    Running,      // Train is currently operating on schedule
    Finished,     // Train completed its route
    Cancelled,    // Train cancelled before departure
    Stopped,      // Train temporarily stopped (maybe due to delay)
    Maintenance   // Train out of service for maintenance
}