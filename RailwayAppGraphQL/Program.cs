using Microsoft.EntityFrameworkCore;
using RailwayAppGraphQL.Data;
using RailwayAppGraphQL.Extensions;
using RailwayAppGraphQL.GraphQL.Queries;

var builder = WebApplication.CreateBuilder(args);

// Register DbContext (for HotChocolate)
builder.Services.AddPooledDbContextFactory<ApplicationDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("MariaDB");
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

// Add GraphQL server
builder.Services
    .AddGraphQLServer()
    .RegisterDbContextFactory<ApplicationDbContext>()
    .AddQueryType<Query>()
    .AddTypes(typeof(TrainQueries), typeof(StationQueries), typeof(TicketQueries))
    .AddProjections(); // select only required fields not all of them
//  .AddFiltering()
//  .AddSorting();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
    // applying migrations automatically
    await app.ApplyMigrationAsync();
else
    app.UseHsts();

app.UseHttpsRedirection();

app.MapGraphQL();

app.MapGet("/", () => "Hello World!");

await app.RunAsync();