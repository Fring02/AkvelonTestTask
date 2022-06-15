using System.ComponentModel.DataAnnotations;
using Domain.Entities.Base;

namespace Domain.Entities;

public class Task : BaseEntity<Guid>
{
    [MaxLength(255)]
    public string Name { get; set; }
    public string? Description { get; set; }
    public TaskStatus Status { get; set; }

    /// <summary>
    /// Get the string value of TaskStatus enum. Is used without Enum.GetName() because of more proper value
    /// </summary>
    /// <param name="status">The TaskStatus value</param>
    /// <returns></returns>
    public string GetStatusName(TaskStatus status) =>
        status switch
        {
            TaskStatus.Done => "Done",
            TaskStatus.InProgress => "In progress",
            TaskStatus.ToDo => "To do",
            _ => ""
        };
    public int Priority { get; set; }
    
    //ProjectId is nullable, because tasks may exist without any project, or "empty" tasks, just with blank project id
    public Guid? ProjectId { get; set; }
    public Project? Project { get; set; }
}