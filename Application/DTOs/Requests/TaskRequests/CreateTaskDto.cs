using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Requests.TaskRequests
{
    public class CreateTaskDto
    {
        public int RequestId { get; set; }
        public int AdminId { get; set; }
        public DateTime DueDate { get; set; }
        public string? Notes { get; set; }
    }

}
