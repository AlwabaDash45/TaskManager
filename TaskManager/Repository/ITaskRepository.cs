using TaskManager.Models;

namespace TaskManager.Repository
{
    public interface ITaskRepository
    {
        Task<IEnumerable<TaskItem>> GetAllTasksAsync();
        Task<TaskItem> GetTaskByIdAsync(int id);
        Task<IEnumerable<TaskItem>> GetTasksByUserIdAsync(string userId);
        Task AddTaskAsync(TaskItem item);
        Task UpdateTaskAsync(TaskItem item);
        Task DeleteTaskAsync(int id);
       
    }
}
