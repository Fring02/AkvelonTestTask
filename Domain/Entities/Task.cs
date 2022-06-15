using System.ComponentModel.DataAnnotations;
using Domain.Entities.Base;

namespace Domain.Entities;

public class Task : BaseEntity<Guid>
{
    [MaxLength(255)]
    public string Name { get; set; }
    public string? Description { get; set; }
    public TaskStatus Status { get; set; }

    public string GetStatusName(TaskStatus status) =>
        status switch
        {
            TaskStatus.Done => "Done",
            TaskStatus.InProgress => "In progress",
            TaskStatus.ToDo => "To do",
            _ => ""
        };
    public int Priority { get; set; }
    public Guid? ProjectId { get; set; }
    public Project? Project { get; set; }
}