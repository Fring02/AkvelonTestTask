namespace Domain.Dtos.Projects;

public class ProjectsSortDto{
    public bool ByStart { get; set; }
    public bool ByCompletion { get; set; }
    public bool ByStatus { get; set; }
    public bool ByPriority { get; set; }
}