using FluentValidation;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using RailwayAppGraphQL.Consumers.Tickets;
using RailwayAppGraphQL.Data;
using RailwayAppGraphQL.Extensions;
using RailwayAppGraphQL.GraphQL.Mutations;
using RailwayAppGraphQL.GraphQL.Queries;

var builder = WebApplication.CreateBuilder(args);

// Register DbContext (for HotChocolate)
builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("MariaDB");
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

// Add validators for FluentValidation
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
//builder.Services.AddScoped<IValidator<CreateStopInput>, CreateStopInputValidator>(); // explicit implementation

// Add GraphQL server
builder.Services
    .AddGraphQLServer()
    .RegisterDbContextFactory<ApplicationDbContext>()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddTypes(typeof(TrainQueries), typeof(StationQueries), typeof(TicketQueries), typeof(StopQueries),
        typeof(TrainMutations), typeof(StationMutations), typeof(TicketMutations), typeof(StopMutations))
    .ModifyPagingOptions(pagingOptions =>
    {
        pagingOptions.DefaultPageSize = 5;
        pagingOptions.MaxPageSize = 10;
        pagingOptions.AllowBackwardPagination = false;
        // pagingOptions.RequirePagingBoundaries = true; // clients need to specify either first, last or take.
    })
    .AddProjections(); // select only required fields not all of them
//  .AddFiltering()
//  .AddSorting();

// Add MassTransit
builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ReceiveEndpoint("ticket-created-queue", e => { e.ConfigureConsumer<TicketCreatedConsumer>(context); });
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // applying migrations automatically
    await app.ApplyMigrationAsync();

    // Seed database (run once at startup)
    await app.SeedDatabaseAsync();
}
else
{
    app.UseHsts();
}

app.UseHttpsRedirection();

app.MapGraphQL();

app.MapGet("/", () => "Hello World!");

await app.RunAsync();