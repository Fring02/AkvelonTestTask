using Domain.Entities;
using Domain.Enums;

namespace Application.Services.Filter;

public class ProjectsSortBuilder
{
    private readonly IQueryable<Project> _projects;
    public IOrderedQueryable<Project>? Result { get; private set; }
    
    //This is used for the first sorting where the ThenBy clause cannot be used due to absence of first sorting. After adding first sorting,
    //the flag is put and the next sorting is used with ThenBy clause
    private bool _orderDefined;
    public ProjectsSortBuilder(IQueryable<Project> projects)
    {
        _projects = projects;
    }
    /// <summary>
    /// Append sorting by status to projects
    /// </summary>
    /// <param name="byStatus">whether to sort by status or not</param>
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
    /// <summary>
    /// Append sorting by priority to projects
    /// </summary>
    /// <param name="byStatus">whether to sort by priority or not</param>
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
    
    /// <summary>
    /// Append sorting by start and end dates to projects. The sorting is done by start date, by end date, or by start and then end dates
    /// </summary>
    /// <param name="byStatus">whether to sort by dates or not</param>
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