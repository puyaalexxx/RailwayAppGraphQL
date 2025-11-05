using HotChocolate.AspNetCore.Voyager;
using Microsoft.EntityFrameworkCore;
using RailwayAppGraphQL.Data;
using RailwayAppGraphQL.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Register DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("MariaDB");

    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)
    );
});

// Add GraphQL server
builder.Services
    .AddGraphQLServer();
// .AddQueryType<Query>()
// .AddProjections()   // optional: for IQueryable
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

app.UseVoyager();

app.MapGet("/", () => "Hello World!");

await app.RunAsync();