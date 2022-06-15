using Domain.Dtos.Tasks;
using Domain.Interfaces.Dtos;

namespace Domain.Dtos.Projects;

public class ProjectDetailedViewDto<TId> : IDto<TId>
{
    public TId Id { get; set; }
    public string Name { get; set; }
    public string StartDate { get; set; }
    public string CompletionDate { get; set; }
    public string Status { get; set; }
    public int Priority { get; set; }
    public ICollection<TaskViewDto<TId>> Tasks { get; set; }
}