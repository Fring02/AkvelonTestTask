using Domain.Dtos.Projects;
using Domain.Dtos.Tasks;

namespace Domain.Interfaces.Services;

public interface IProjectsService : 
    ICrudService<Guid, ProjectCreateDto<Guid>, ProjectUpdateDto<Guid>, ProjectViewDto<Guid>, ProjectDetailedViewDto<Guid>>
{
    System.Threading.Tasks.Task DeleteTasksAsync(Guid projectId, CancellationToken token = default, params Guid[] taskIds);
    System.Threading.Tasks.Task AddTasksAsync(Guid projectId, CancellationToken token = default, params Guid[] taskIds);

    Task<IEnumerable<ProjectViewDto<Guid>>> FilterAsync(ProjectsFilterDto? filter, ProjectsSortDto? sort,
        CancellationToken token = default);
}