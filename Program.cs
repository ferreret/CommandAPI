using CommandAPI.Data;
using CommandAPI.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlDbConnection"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Get Single
app.MapGet("api/v1/commands/{commandId}", async (string commandId, AppDbContext dbContext) =>
{
    var command = await dbContext.Commands.FirstOrDefaultAsync(c => c.CommandId == commandId);
    if (command == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(command);
});

// Get All
app.MapGet("api/v1/commands", async (AppDbContext dbContext) =>
{
    var commands = await dbContext.Commands.ToListAsync();
    return Results.Ok(commands);
});

// Create
app.MapPost("api/v1/commands", async (AppDbContext dbContext, Command cmd) =>
{
    await dbContext.Commands.AddAsync(cmd);
    await dbContext.SaveChangesAsync();
    return Results.Created($"/api/v1/commands/{cmd.CommandId}", cmd);
});

// Update
app.MapPut("api/v1/commands/{commandId}", async (AppDbContext dbContext, string commandId, Command cmd) =>
{
    var command = await dbContext.Commands.FirstOrDefaultAsync(c => c.CommandId == commandId);
    if (command == null)
    {
        return Results.NotFound();
    }

    // Lo siguiente está mal hecho, se tendrían que utilizar DTO's
    command.HowTo = cmd.HowTo;
    command.Platform = cmd.Platform;
    command.CommandLine = cmd.CommandLine;
    await dbContext.SaveChangesAsync();
    return Results.Ok(command);
});

// Delete
app.MapDelete("api/v1/commands/{commandId}", async (AppDbContext dbContext, string commandId) =>
{
    var command = await dbContext.Commands.FirstOrDefaultAsync(c => c.CommandId == commandId);
    if (command == null)
    {
        return Results.NotFound();
    }

    dbContext.Commands.Remove(command);
    await dbContext.SaveChangesAsync();
    return Results.NoContent();
});

app.Run();


