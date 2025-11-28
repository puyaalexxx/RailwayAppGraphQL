using Microsoft.EntityFrameworkCore;
using RailwayAppGraphQL.Data;
using RailwayAppGraphQL.DTOs;

namespace RailwayAppGraphQL.Helpers;

public static class TicketHelpers
{
    /// <summary>
    ///     Gets train info for ticket
    /// </summary>
    /// <param name="dbContext">Database context</param>
    /// <param name="trainId">Train ID</param>
    /// <returns>Train info for ticket</returns>
    public static async Task<TicketTrainInfoDto> GetTicketTrainInfoAsync(ApplicationDbContext dbContext, Guid trainId)
    {
        // Load train with stops and stations info
        var train = await dbContext.Trains
            .Include(t => t.Stops.OrderBy(s => s.ArrivalTimeUtc))
            .ThenInclude(s => s.Station)
            .FirstOrDefaultAsync(t => t.Id == trainId);

        DateTime departureTime;
        DateTime arrivalTime;
        string departureStation;
        string arrivalStation;

        // we need to get the departure and arrival station names if train has only one stop or more than one stop
        // train exists because we checked it in the CreateTicketInputValidator
        if (train!.Stops.Count == 1)
        {
            var stop = train.Stops.First();
            departureTime = stop.DepartureTimeUtc;
            arrivalTime = stop.ArrivalTimeUtc;
            departureStation = stop.Station.Name;
            arrivalStation = stop.Station.Name;
        }
        else
        {
            var firstStop = train.Stops.First();
            var lastStop = train.Stops.Last();

            departureTime = firstStop.DepartureTimeUtc;
            arrivalTime = lastStop.ArrivalTimeUtc;
            departureStation = firstStop.Station.Name;
            arrivalStation = lastStop.Station.Name;
        }

        return new TicketTrainInfoDto(
            train.Number, train.Name, departureTime,
            arrivalTime, departureStation, arrivalStation);
    }
}