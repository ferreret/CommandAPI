using CommandAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CommandAPI.Data
{
    public class SqlCommandRepo : ICommandRepo
    {
        private readonly AppDbContext _dbContext;

        public SqlCommandRepo(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CreateCommandAsync(Command cmd)
        {
            ArgumentNullException.ThrowIfNull(cmd);

            await _dbContext.Commands.AddAsync(cmd);
        }

        public void DeleteCommand(Command cmd)
        {
            ArgumentNullException.ThrowIfNull(cmd);

            _dbContext.Commands.Remove(cmd);
        }

        public async Task<IEnumerable<Command?>> GetAllCommandsAsync()
        {
            return await _dbContext.Commands.ToListAsync();
        }

        public async Task<Command?> GetCommandByIdAsync(string commandId)
        {
            return await _dbContext.Commands.SingleOrDefaultAsync(c => c.CommandId == commandId);
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public Task UpdateCommandAsync(Command cmd)
        {
            Console.WriteLine("--> UpdateCommandAsync in SqlCommandRepo called redundantly ...");
            return Task.CompletedTask;
        }
    }
}
