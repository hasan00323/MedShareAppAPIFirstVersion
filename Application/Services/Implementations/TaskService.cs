using Application.DTOs.Requests.TaskRequests;
using Application.Repositories.Interfaces;
using Application.Services.Interfaces;
using Domain.Entities;
using Domain.Entities.Enum;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Implementations
{
    public class TaskService : ITaskService
    {
        private readonly IGenericRepository<RequestTask> _taskRepo;
        public TaskService(IGenericRepository<RequestTask> taskRepo)
        {
            _taskRepo = taskRepo;
        }

        public async Task AddTaskAsync(CreateTaskDto dto)
        {
            var task = new RequestTask
            {
                AdminId = dto.AdminId,
                DueDate = dto.DueDate,
                Notes = dto.Notes,
                Status = TasksStatus.Pending
            };

            await _taskRepo.Insert(task);
            await _taskRepo.SaveChanges();
        }

        public async Task CancelTaskAsync(int taskId)
        {
            var task = await _taskRepo.GetAll().FirstOrDefaultAsync(t => t.TaskId == taskId);

            if (task == null)  throw new Exception("Task not found");

            task.Status = TasksStatus.Cancelled;

            await _taskRepo.SaveChanges();
        }

        public async Task<IEnumerable<RequestTask>> GetAllTasksAsync()
        {
            return await _taskRepo.GetAll().ToListAsync();
        }

        public async Task<RequestTask?> GetTaskDetailsAsync(int taskId)
        {
            return await _taskRepo.GetAll().FirstOrDefaultAsync(t => t.TaskId == taskId);
        }
    }

}
