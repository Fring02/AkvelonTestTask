using Domain.Entities.Base;
using Domain.Enums;

namespace Domain.Entities;

public class Project : BaseEntity<Guid>
{
    public string Name { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime CompletionDate { get; set; }
    public ProjectStatus Status { get; set; }

    /// <summary>
    /// Get the string value of ProjectStatus enum. Is used without Enum.GetName() because of more proper value
    /// </summary>
    /// <param name="status">The ProjectStatus value</param>
    /// <returns></returns>
    public string GetStatusName(ProjectStatus status)
    {
        return status switch
        {
            ProjectStatus.Active => "Active",
            ProjectStatus.NotStarted => "Not started",
            ProjectStatus.Completed => "Completed",
            _ => ""
        };
    }
    public int Priority { get; set; }
    public ICollection<Task> Tasks { get; set; }
}