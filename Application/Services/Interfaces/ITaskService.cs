
using Application.DTOs.Requests.TaskRequests;
using Domain.Entities;

namespace Application.Services.Interfaces
{
    public interface ITaskService
    {
        Task AddTaskAsync(CreateTaskDto dto);
        Task CancelTaskAsync(int taskId);
        Task<IEnumerable<RequestTask>> GetAllTasksAsync();
        Task<RequestTask?> GetTaskDetailsAsync(int taskId);
    }
}
