namespace RailwayAppGraphQL.DTOs;

public record TicketTrainInfoDto(
    string TrainNumber,
    string TrainName,
    DateTime DepartureTime,
    DateTime ArrivalTime,
    string DepartureStation,
    string ArrivalStation);