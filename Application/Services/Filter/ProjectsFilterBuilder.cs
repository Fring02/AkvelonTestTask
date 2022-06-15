using Domain.Entities;
using Domain.Enums;

namespace Application.Services.Filter;

public class ProjectsFilterBuilder
{
    public IQueryable<Project> Result { get; private set; }
    public ProjectsFilterBuilder(IQueryable<Project> projects)
    {
        Result = projects;
    }

    public ProjectsFilterBuilder WithName(string? name)
    {
        if(!string.IsNullOrEmpty(name))
            Result = Result.Where(p => p.Name == name);
        return this;
    }
    public ProjectsFilterBuilder WithStatus(ProjectStatus? status)
    {
        if(status != null)
            Result = Result.Where(p => p.Status == status);
        return this;
    }
    public ProjectsFilterBuilder WithPriority(int? priority)
    {
        if(priority != null)
            Result = Result.Where(p => p.Priority == priority);
        return this;
    }
    public ProjectsFilterBuilder WithDates(DateTime? startDate, DateTime? endDate)
    {
        if (startDate != null && endDate != null)
            Result = Result.Where(p => p.StartDate >= startDate && p.CompletionDate <= endDate);
        else if (startDate != null)
            Result = Result.Where(p => p.StartDate >= startDate);
        else if(endDate != null)
            Result = Result.Where(p => p.CompletionDate <= endDate);
        return this;
    }
}