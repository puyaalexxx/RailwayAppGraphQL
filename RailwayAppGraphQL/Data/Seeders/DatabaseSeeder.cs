using Bogus;
using Microsoft.EntityFrameworkCore;
using RailwayAppGraphQL.Models;
using RailwayAppGraphQL.Models.Tickets;
using RailwayAppGraphQL.Models.Trains;
using Type = RailwayAppGraphQL.Models.Trains.Type;

namespace RailwayAppGraphQL.Data.Seeders;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(ApplicationDbContext db)
    {
        if (await db.Stations.AnyAsync())
            return; // Prevent double-seeding

        // -----------------------------------------
        // 1. Stations
        // -----------------------------------------
        var stationFaker = new Faker<Station>()
            .RuleFor(x => x.Id, _ => Guid.NewGuid())
            .RuleFor(x => x.Name, f => f.Address.City() + " Station")
            .RuleFor(x => x.Address, f => f.Address.FullAddress())
            .RuleFor(x => x.HasWc, f => f.Random.Bool())
            .RuleFor(x => x.HasCoffeeMachine, f => f.Random.Bool())
            .RuleFor(x => x.HasWaitingRoom, f => f.Random.Bool());

        var stations = stationFaker.Generate(15);
        await db.Stations.AddRangeAsync(stations);

        // -----------------------------------------
        // 2. Trains
        // -----------------------------------------
        var trainFaker = new Faker<Train>()
            .RuleFor(x => x.Id, _ => Guid.NewGuid())
            .RuleFor(x => x.Name, f => f.Vehicle.Manufacturer() + " Express")
            .RuleFor(x => x.Number, f => f.Random.AlphaNumeric(10).ToUpper())
            .RuleFor(x => x.Seats, f => f.Random.Int(60, 300))
            .RuleFor(x => x.Type, f => f.PickRandom<Type>())
            .RuleFor(x => x.Status, f => f.PickRandom<Status>())
            .RuleFor(x => x.Stops, _ => new List<Stop>())
            .RuleFor(x => x.Tickets, _ => new List<Ticket>());

        var trains = trainFaker.Generate(10);
        await db.Trains.AddRangeAsync(trains);

        // -----------------------------------------
        // 3. Stops (Each train gets 3–6 stops)
        // -----------------------------------------
        var stopId = 1;

        foreach (var train in trains)
        {
            var stopCount = new Random().Next(3, 6);
            var routeStations = stations.OrderBy(_ => Guid.NewGuid()).Take(stopCount).ToList();
            var currentTime = DateTime.UtcNow.AddHours(-5);

            foreach (var st in routeStations)
            {
                var arrival = currentTime;
                var departure = arrival.AddMinutes(new Random().Next(20, 90));

                train.Stops.Add(new Stop
                {
                    Id = Guid.NewGuid(),
                    StationId = st.Id,
                    ArrivalTimeUtc = arrival,
                    DepartureTimeUtc = departure
                });

                currentTime = departure.AddMinutes(new Random().Next(10, 60));
            }
        }

        // -----------------------------------------
        // 4. Tickets (random amount per train)
        // -----------------------------------------
        var ticketFaker = new Faker<Ticket>()
            .RuleFor(x => x.Id, _ => Guid.NewGuid())
            .RuleFor(x => x.Number, f => "TKT-" + f.Random.AlphaNumeric(12).ToUpper())
            .RuleFor(x => x.PassengerName, f => f.Name.FullName())
            .RuleFor(x => x.PassengerEmail, f => f.Internet.Email())
            .RuleFor(x => x.SeatNumber, f => f.Random.Int(1, 300).ToString())
            .RuleFor(x => x.Price, f => f.Random.Decimal(15, 150))
            .RuleFor(x => x.Currency, f => f.PickRandom<Currency>())
            .RuleFor(x => x.PurchasedAtUtc, f => f.Date.RecentOffset(15).UtcDateTime);

        foreach (var train in trains)
        {
            var ticketCount = new Random().Next(30, train.Seats);

            var tickets = ticketFaker.Generate(ticketCount);

            foreach (var t in tickets)
                t.TrainId = train.Id;

            await db.Tickets.AddRangeAsync(tickets);
        }

        // Save all changes
        await db.SaveChangesAsync();
    }
}