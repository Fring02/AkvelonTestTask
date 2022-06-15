namespace Domain.Dtos.Projects;

//The SortDto's flags define the sort of various fields in projects
public class ProjectsSortDto{
    public bool ByStart { get; set; }
    public bool ByCompletion { get; set; }
    public bool ByStatus { get; set; }
    public bool ByPriority { get; set; }
}