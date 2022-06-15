using Domain.Entities;
using Domain.Enums;

namespace Application.Services.Filter;

public class ProjectsSortBuilder
{
    private readonly IQueryable<Project> _projects;
    public IOrderedQueryable<Project>? Result { get; private set; }
    private bool _orderDefined;
    public ProjectsSortBuilder(IQueryable<Project> projects)
    {
        _projects = projects;
    }
    
    public ProjectsSortBuilder ByStatus(bool byStatus)
    {
        if (byStatus)
        {
            if (!_orderDefined)
            {
                _orderDefined = true;
                Result = _projects.OrderBy(p => p.Status);
            }
            else
                Result = Result.ThenBy(p => p.Status);
        }
        return this;
    }
    public ProjectsSortBuilder ByPriority(bool byPriority)
    {
        if (byPriority)
        {
            if (!_orderDefined)
            {
                Result = _projects.OrderBy(p => p.Priority);
                _orderDefined = true;
            }
            else
                Result = Result.ThenBy(p => p.Priority);
        }
        return this;
    }
    public ProjectsSortBuilder ByDates(bool byStart, bool byEnd)
    {
        if (byStart && byEnd)
        {
            if (!_orderDefined)
            {
                Result = _projects.OrderBy(p => p.StartDate).ThenBy(p => p.CompletionDate);
                _orderDefined = true;
            }
            else
                Result = Result.ThenBy(p => p.StartDate).ThenBy(p => p.CompletionDate);
        }
        else if (byStart)
        {
            if (!_orderDefined)
            {
                Result = _projects.OrderBy(p => p.StartDate);
                _orderDefined = true;
            }
            else
                Result = Result.ThenBy(p => p.StartDate);
        }
        else if (byEnd)
        {
            if (!_orderDefined)
            {
                Result = _projects.OrderBy(p => p.StartDate);
                _orderDefined = true;
            }
            else
                Result = Result.ThenBy(p => p.CompletionDate);
        }
        return this;
    }
}