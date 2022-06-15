using Application.Services.Base;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Data.Contexts;
using Domain.Dtos.Tasks;
using Domain.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
namespace Application.Services.Tasks;

public class TasksService : CrudService<Task, TaskCreateDto<Guid>, TaskUpdateDto<Guid>, TaskViewDto<Guid>, TaskDetailedViewDto<Guid>>, ITasksService
{
    public TasksService(ApplicationContext context, IMapper mapper) : base(context, mapper)
    {
    }

    public override async Task<TaskCreateDto<Guid>> CreateAsync(TaskCreateDto<Guid> createEntity, CancellationToken token)
    {
        //Check if project exists
        if (!await _context.Projects.AnyAsync(p => p.Id == createEntity.ProjectId, token))
            throw new ArgumentException($"Project with id {createEntity.ProjectId} does not exist");
        //Check if any task with given name and project id is present
        if (await _context.Tasks.AnyAsync(t => t.ProjectId == createEntity.ProjectId && t.Name == createEntity.Name,
                cancellationToken: token))
            throw new ArgumentException("Task with this name is already in the project");
        return await base.CreateAsync(createEntity, token);
    }

    public override async System.Threading.Tasks.Task UpdateAsync(TaskUpdateDto<Guid> updateEntity, CancellationToken token)
    {
        //Find task entity with id
        var entity = await _context.Tasks.FindAsync(new object?[] { updateEntity.Id }, cancellationToken: token);
        if (entity == null) throw new ArgumentException($"Task with id {updateEntity.Id} is not found");
        if (!string.IsNullOrEmpty(updateEntity.Description)) entity.Description = updateEntity.Description;
        if (!string.IsNullOrEmpty(updateEntity.Name)) entity.Name = updateEntity.Name;
        if (updateEntity.Status.HasValue && updateEntity.Status != entity.Status) entity.Status = updateEntity.Status.Value;
        
        //Here we can remove the task out of the project: if project id is null, then set null; otherwise,
        //check if it was not changed => equals to default, then do nothing; otherwise, update the project id
        if (updateEntity.ProjectId == null)
            entity.ProjectId = null;
        else if (updateEntity.ProjectId != Guid.Empty) {
           if (await _context.Projects.AnyAsync(p => p.Id == updateEntity.ProjectId, token))
               entity.ProjectId = updateEntity.ProjectId;
           else
               throw new ArgumentException($"The project with id {updateEntity.ProjectId} is not found");
        }
        //Check the priority, update if new value is present
        if (updateEntity.Priority.HasValue && updateEntity.Priority != entity.Priority) entity.Priority = updateEntity.Priority.Value;
        await _context.SaveChangesAsync(token);
    }

    public async Task<IEnumerable<TaskViewDto<Guid>>> GetByProjectAsync(Guid projectId, CancellationToken token)
        => await _context.Tasks.AsNoTracking().Where(t => t.ProjectId == projectId)
            .ProjectTo<TaskViewDto<Guid>>(_mapper.ConfigurationProvider).ToListAsync(token);
}