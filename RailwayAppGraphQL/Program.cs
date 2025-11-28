using FluentValidation;
using Microsoft.EntityFrameworkCore;
using QuestPDF;
using QuestPDF.Infrastructure;
using RailwayAppGraphQL.Data;
using RailwayAppGraphQL.Extensions;

Settings.License = LicenseType.Community;

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

builder.Services.AddGraphQL();

builder.Services.AddMassTransit();

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