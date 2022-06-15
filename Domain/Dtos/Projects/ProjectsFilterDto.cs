using Domain.Enums;

namespace Domain.Dtos.Projects;

public class ProjectsFilterDto
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public ProjectStatus? Status { get; set; }
    public int? Priority { get; set; }
    public string? Name { get; set; }
}