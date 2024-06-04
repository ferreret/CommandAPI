using System.Text.Json;
using CommandAPI.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using StackExchange.Redis;

namespace CommandAPI.Data
{
    public class RedisCommandRepo : ICommandRepo
    {
        private readonly IConnectionMultiplexer _redisConnection;

        public RedisCommandRepo(IConnectionMultiplexer redisConnection)
        {
            _redisConnection = redisConnection;
        }

        public async Task CreateCommandAsync(Command cmd)
        {
            ArgumentNullException.ThrowIfNull(cmd);

            var db = _redisConnection.GetDatabase();
            var serialCommand = JsonSerializer.Serialize(cmd);
            await db.HashSetAsync($"commands", [new HashEntry(cmd.CommandId, serialCommand)]);
        }

        public void DeleteCommand(Command cmd)
        {
            ArgumentNullException.ThrowIfNull(cmd);

            var db = _redisConnection.GetDatabase();
            db.HashDelete("commands", cmd.CommandId);
        }

        public async Task<IEnumerable<Command?>> GetAllCommandsAsync()
        {
            var db = _redisConnection.GetDatabase();

            var completeSet = await db.HashGetAllAsync("commands");

            if (completeSet.Length > 0)
            {
                return Array.ConvertAll(completeSet, x => JsonSerializer.Deserialize<Command>(x.Value!));
            }

            List<Command> empty = new List<Command>();

            return empty;
        }

        public async Task<Command?> GetCommandByIdAsync(string commandId)
        {
            var db = _redisConnection.GetDatabase();

            var command = await db.HashGetAsync("commands", commandId);

            if (!string.IsNullOrEmpty(command))
            {
                return JsonSerializer.Deserialize<Command>(command!);
            }

            return null;
        }

        public async Task SaveChangesAsync()
        {
            Console.WriteLine("--> SaveChangesAsync in RedisCommandRepo called reduntaly...");
            await Task.CompletedTask;
        }

        public async Task UpdateCommandAsync(Command cmd)
        {
            var db = _redisConnection.GetDatabase();

            var serialCommand = JsonSerializer.Serialize(cmd);
            await db.HashSetAsync($"commands", [new HashEntry(cmd.CommandId, serialCommand)]);
        }
    }
}