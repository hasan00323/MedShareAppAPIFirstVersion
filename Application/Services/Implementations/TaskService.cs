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
        private readonly IGenericRepository<Request> _requestRepo;

        public TaskService(IGenericRepository<RequestTask> taskRepo,IGenericRepository<Request> requestRepo)
        {
            _taskRepo = taskRepo;
            _requestRepo = requestRepo;
        }

        public async Task AddTaskAsync(CreateTaskDto dto)
        {
            var request = await _requestRepo.GetById(dto.RequestId);

            if (request == null)
                throw new Exception("Request not found");

            if (request.Status != StatusDonation.Pending)
                throw new Exception("Request already processed");

            var task = new RequestTask
            {
                RequestId = dto.RequestId,
                AdminId = dto.AdminId,
                AssignedDate = DateTime.UtcNow,
                DueDate = dto.DueDate,
                Notes = dto.Notes,
                Status = TasksStatus.Pending
            };

            await _taskRepo.Insert(task);

            request.Status = StatusDonation.Approved;

            await _taskRepo.SaveChanges();
        }

        public async Task CancelTaskAsync(int taskId)
        {
            var task = await _taskRepo.GetAll().Include(t => t.Request)
                .FirstOrDefaultAsync(t => t.TaskId == taskId);

            if (task == null)
                throw new Exception("Task not found");

            task.Status = TasksStatus.Cancelled;

            if (task.Request != null)
                task.Request.Status = StatusDonation.Pending;

            await _taskRepo.SaveChanges();
        }

        public async Task<IEnumerable<RequestTask>> GetAllTasksAsync()
        {
            return await _taskRepo.GetAll().Include(t => t.Request)
                        .Include(t => t.Admin).ToListAsync();
        }

        public async Task<RequestTask?> GetTaskDetailsAsync(int taskId)
        {
            return await _taskRepo.GetAll().Include(t => t.Request)
                .Include(t => t.Admin).FirstOrDefaultAsync(t => t.TaskId == taskId);
        }
    }

}
