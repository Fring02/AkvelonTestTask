using Domain.Dtos.Projects;
using Domain.Interfaces.Dtos;

namespace Domain.Dtos.Tasks;

public record TaskDetailedViewDto<TId> : IDto<TId>
{
    public TId Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string Status { get; set; }
    public int Priority { get; set; }
    public Guid? ProjectId { get; set; }
    public ProjectViewDto<TId>? Project { get; set; }
}