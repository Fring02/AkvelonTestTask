using Domain.Enums;

namespace Domain.Dtos.Projects;

//The filter values in filter dto are used for filtering by various fields in projects
public class ProjectsFilterDto
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public ProjectStatus? Status { get; set; }
    public int? Priority { get; set; }
    public string? Name { get; set; }
}