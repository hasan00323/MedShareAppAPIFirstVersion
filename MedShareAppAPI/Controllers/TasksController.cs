using Application.DTOs.Requests.TaskRequests;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MedShareAppAPI.Controllers
{
    [ApiController]
    [Route("api/tasks")]
    [Authorize(Roles = "Admin")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpPost("assign")]
        public async Task<IActionResult> AssignTask([FromBody] CreateTaskDto dto)
        {
            await _taskService.AddTaskAsync(dto);
            return Ok("Task assigned successfully.");
        }

        [HttpPut("cancel/{taskId}")]
        public async Task<IActionResult> CancelTask(int taskId)
        {
            await _taskService.CancelTaskAsync(taskId);
            return Ok("Task cancelled successfully.");
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTasks()
        {
            var tasks = await _taskService.GetAllTasksAsync();
            return Ok(tasks);
        }

        [HttpGet("{taskId}")]
        public async Task<IActionResult> GetTaskDetails(int taskId)
        {
            var task = await _taskService.GetTaskDetailsAsync(taskId);

            if (task == null)
                return NotFound("Task not found.");

            return Ok(task);
        }
    }
}
