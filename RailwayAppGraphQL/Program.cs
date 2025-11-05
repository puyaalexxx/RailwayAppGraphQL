using Microsoft.EntityFrameworkCore;
using RailwayAppGraphQL.Data;

var builder = WebApplication.CreateBuilder(args);

// Register DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("ConnectionStrings");

    options.UseMySql( connectionString, ServerVersion.AutoDetect(connectionString)
    );
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // applying migrations automatically
    //await app.ApplyMigrationAsync();
}
else
{
    app.UseHsts();
}

app.UseHttpsRedirection();

app.MapGet("/", () => "Hello World!");

await app.RunAsync();