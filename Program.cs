using CommandAPI.Data;
using CommandAPI.Models;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlDbConnection"));
});

builder.Services.AddSingleton<IConnectionMultiplexer>(
    opt => ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("RedisConnection")!)
);

// builder.Services.AddScoped<ICommandRepo, SqlCommandRepo>();
builder.Services.AddScoped<ICommandRepo, RedisCommandRepo>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Get Single
app.MapGet("api/v1/commands/{commandId}", async (string commandId, ICommandRepo repo) =>
{
    var command = await repo.GetCommandByIdAsync(commandId);
    if (command == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(command);
});

// Get All
app.MapGet("api/v1/commands", async (ICommandRepo repo) =>
{
    var commands = await repo.GetAllCommandsAsync();
    return Results.Ok(commands);
});

// Create
app.MapPost("api/v1/commands", async (ICommandRepo repo, Command cmd) =>
{
    await repo.CreateCommandAsync(cmd);
    await repo.SaveChangesAsync();
    return Results.Created($"/api/v1/commands/{cmd.CommandId}", cmd);
});

// Update
app.MapPut("api/v1/commands/{commandId}", async (ICommandRepo repo, string commandId, Command cmd) =>
{
    var command = await repo.GetCommandByIdAsync(commandId);
    if (command == null)
    {
        return Results.NotFound();
    }

    command.HowTo = cmd.HowTo;
    command.Platform = cmd.Platform;
    command.CommandLine = cmd.CommandLine;

    await repo.UpdateCommandAsync(command);
    await repo.SaveChangesAsync();
    return Results.NoContent();
});

// Delete
app.MapDelete("api/v1/commands/{commandId}", async (ICommandRepo repo, string commandId) =>
{
    var command = await repo.GetCommandByIdAsync(commandId);
    if (command == null)
    {
        return Results.NotFound();
    }

    repo.DeleteCommand(command);
    await repo.SaveChangesAsync();
    return Results.NoContent();
});

app.Run();