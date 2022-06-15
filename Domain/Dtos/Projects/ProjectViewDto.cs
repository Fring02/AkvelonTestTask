using Domain.Interfaces.Dtos;

namespace Domain.Dtos.Projects;

public record ProjectViewDto<TId> : IDto<TId>
{
    public TId Id { get; set; }
    public string Name { get; set; }
    public string StartDate { get; set; }
    public string CompletionDate { get; set; }
    public string Status { get; set; }
    public int Priority { get; set; }
}