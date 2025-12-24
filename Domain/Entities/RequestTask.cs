using Domain.Entities.Enum;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class RequestTask
    {
        [Key]
        public int TaskId { get; set; }

        public int RequestId { get; set; }
        public Request Request { get; set; } = null!;

        public int? DonationId { get; set; }
        public Donation? Donation { get; set; }

        public int AdminId { get; set; }
        public User Admin { get; set; } = null!;

        public DateTime AssignedDate { get; set; }
        public DateTime DueDate { get; set; }

        public TasksStatus Status { get; set; } = TasksStatus.Pending;

        public string? Notes { get; set; }
    }
}
