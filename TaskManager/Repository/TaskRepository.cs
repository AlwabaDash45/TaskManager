using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using TaskManager.Data;
using TaskManager.Models;

namespace TaskManager.Repository
{
    public class TaskRepository : ITaskRepository
    {
        private readonly ApplicationDbContext _context;

        public TaskRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddTaskAsync(TaskItem task)
        {
             _context.TaskItems.Add(task);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTaskAsync(int id)
        {
            var task = await _context.TaskItems.FindAsync(id);
            if (task != null) 
            {
                _context.TaskItems.Remove(task);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<TaskItem>> GetAllTasksAsync()
        {
            return await _context.TaskItems.ToListAsync();
        }

        public async Task<TaskItem> GetTaskByIdAsync(int id)
        {
            return await _context.TaskItems
                .Include(t => t.AssignedToUser) 
                .FirstOrDefaultAsync(t => t.Id == id);
        }


        public async Task<IEnumerable<TaskItem>> GetTasksByUserIdAsync(string userId) 
        {
            return await _context.TaskItems
                .Where(t => t.AssignedToUserId == userId)
                .Include(t => t.AssignedToUser)
                .ToListAsync();
        }

        public async Task UpdateTaskAsync(TaskItem item)
        {
            var existingTask = await _context.TaskItems.FindAsync(item.Id);

            if (existingTask != null)
            {
                existingTask.Title = item.Title;
                existingTask.Description = item.Description;
                existingTask.IsCompleted = item.IsCompleted;
                existingTask.AssignedToUserId = item.AssignedToUserId; 

                await _context.SaveChangesAsync();
            }
        }

    }
}
