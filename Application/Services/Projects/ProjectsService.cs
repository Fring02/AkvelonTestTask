using Application.Services.Base;
using Application.Services.Filter;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Data.Contexts;
using Domain.Dtos.Projects;
using Domain.Entities;
using Domain.Interfaces.Services;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Projects;

public class ProjectsService : 
    CrudService<Project, ProjectCreateDto<Guid>, ProjectUpdateDto<Guid>, ProjectViewDto<Guid>, ProjectDetailedViewDto<Guid>>, IProjectsService
{
    private ProjectsFilterBuilder _filter;
    private ProjectsSortBuilder _sort;
    public ProjectsService(ApplicationContext context, IMapper mapper) : base(context, mapper)
    {
    }

    public override async Task<ProjectCreateDto<Guid>> CreateAsync(ProjectCreateDto<Guid> createEntity, CancellationToken token)
    {
        if (await _context.Projects.AnyAsync(p => p.Name == createEntity.Name, token))
            throw new ArgumentException($"Project with name `{createEntity.Name}` already exists");
        if (createEntity.CompletionDate < createEntity.StartDate)
            throw new ArgumentException("The start date of project must be less than its completion date");
        return await base.CreateAsync(createEntity, token);
    }

    /// <summary>
    /// Update project entity. Update fields where not empty of default
    /// </summary>
    /// <param name="updateEntity">The body of fields of entity to be updated in a DTO format</param>
    /// <param name="token">Cancellation token</param>
    /// <exception cref="ArgumentException">If the project is not found; If the completion date is less the start date</exception>
    public override async System.Threading.Tasks.Task UpdateAsync(ProjectUpdateDto<Guid> updateEntity, CancellationToken token)
    {
        //Finding project by id
        var entity = await _context.Projects.FindAsync(new object?[] { updateEntity.Id }, cancellationToken: token);
        if (entity == null) throw new ArgumentException($"Project with id {updateEntity.Id} is not found");
        if (!string.IsNullOrEmpty(updateEntity.Name)) entity.Name = updateEntity.Name;
        if (updateEntity.Priority != entity.Priority) entity.Priority = updateEntity.Priority;
        if (updateEntity.Status != entity.Status) entity.Status = updateEntity.Status;
        //Checking if start date surpasses completion date
        if (updateEntity.CompletionDate != default && updateEntity.StartDate != default && updateEntity.CompletionDate < updateEntity.StartDate)
                throw new ArgumentException("The start date of project must be less than its completion date");
        if (updateEntity.CompletionDate != default) entity.CompletionDate = updateEntity.CompletionDate;
        if (updateEntity.StartDate != default) entity.StartDate = updateEntity.StartDate;
        await _context.SaveChangesAsync(token);
    }

    public async System.Threading.Tasks.Task DeleteTasksAsync(Guid projectId, CancellationToken token = default, params Guid[] taskIds)
    {
        //Checking if any task by id in array is not present in database
        if (!await _context.Tasks.AnyAsync(t => taskIds.Contains(t.Id), cancellationToken: token))
            throw new ArgumentException("Some tasks do not exist");
        //Check the existence of project by id
        if (!await _context.Projects.AnyAsync(p => p.Id == projectId, cancellationToken: token))
            throw new ArgumentException($"Project with id {projectId} is not found");

        
        var tasksToRemove = _context.Tasks.Where(t => t.ProjectId == projectId && taskIds.Contains(t.Id));
        //Updating tasks by setting projects to null
        await tasksToRemove.ForEachAsync(t => t.ProjectId = null, token);
        _context.Tasks.UpdateRange(tasksToRemove);
        await _context.SaveChangesAsync(token);
    }

    public async System.Threading.Tasks.Task AddTasksAsync(Guid projectId, CancellationToken token = default, params Guid[] taskIds)
    {
        //Checking if any task by id in array is not present in database
        if (!await _context.Tasks.AnyAsync(t => taskIds.Contains(t.Id), cancellationToken: token))
            throw new ArgumentException("Some tasks do not exist");
        //Check the existence of project by id
        if (!await _context.Projects.AnyAsync(p => p.Id == projectId, cancellationToken: token))
            throw new ArgumentException($"Project with id {projectId} is not found");
        var tasksToAdd = _context.Tasks.Where(t => taskIds.Contains(t.Id));
        //Updating tasks by setting projects to given project Id
        await tasksToAdd.ForEachAsync(t => t.ProjectId = projectId, token);
        _context.Tasks.UpdateRange(tasksToAdd);
        await _context.SaveChangesAsync(token);
    }

    public async Task<IEnumerable<ProjectViewDto<Guid>>> FilterAsync(ProjectsFilterDto? filter, ProjectsSortDto? sort, CancellationToken token = default)
    {
        //Setting filter as no tracking because of outputting to user
        var projects = _context.Projects.AsNoTracking();
        if (filter != null && sort != null)
        {
            //first: turn projects into filtered
            _filter = new(_context.Projects);
            projects = _filter.WithName(filter.Name).WithStatus(filter.Status)
                .WithPriority(filter.Priority).WithDates(filter.StartDate, filter.EndDate).Result;
            //then: sort projects
            _sort = new(projects);
            var sortedProjects = _sort.ByStatus(sort.ByStatus).ByPriority(sort.ByPriority)
                .ByDates(sort.ByStart, sort.ByCompletion).Result;
            //if there are some fields to be sorted with: otherwise, just filter the projects without sorting
            if (sortedProjects != null)
                projects = sortedProjects;
        }
        return await projects.ProjectTo<ProjectViewDto<Guid>>(_mapper.ConfigurationProvider).ToListAsync(token);
    }
}