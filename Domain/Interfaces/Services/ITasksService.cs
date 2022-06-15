using Domain.Dtos.Tasks;
namespace Domain.Interfaces.Services;

public interface ITasksService : ICrudService<Guid, TaskCreateDto<Guid>, TaskUpdateDto<Guid>, TaskViewDto<Guid>, TaskDetailedViewDto<Guid>>
{
    Task<IEnumerable<TaskViewDto<Guid>>> GetByProjectAsync(Guid projectId, CancellationToken token);
}