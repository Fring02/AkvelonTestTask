using Domain.Dtos.Tasks;
namespace Domain.Interfaces.Services;

public interface ITasksService : ICrudService<Guid, TaskCreateDto<Guid>, TaskUpdateDto<Guid>, TaskViewDto<Guid>, TaskDetailedViewDto<Guid>>
{
    /// <summary>
    /// Get the list of tasks by specified projectId
    /// </summary>
    /// <param name="projectId">The id of project</param>
    /// <param name="token">Cancellation token</param>
    /// <returns>The list of task view DTOs</returns>
    Task<IEnumerable<TaskViewDto<Guid>>> GetByProjectAsync(Guid projectId, CancellationToken token);
}