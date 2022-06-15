using Domain.Dtos.Projects;
using Domain.Dtos.Tasks;

namespace Domain.Interfaces.Services;

public interface IProjectsService : 
    ICrudService<Guid, ProjectCreateDto<Guid>, ProjectUpdateDto<Guid>, ProjectViewDto<Guid>, ProjectDetailedViewDto<Guid>>
{
    /// <summary>
    /// Delete specified tasks by array ids from the project with projectId. (Update tasks by setting project ids as null)
    /// </summary>
    /// <param name="projectId">The id of project</param>
    /// <param name="token">Cancellation token</param>
    /// <param name="taskIds">the array of task ids</param>
    System.Threading.Tasks.Task DeleteTasksAsync(Guid projectId, CancellationToken token = default, params Guid[] taskIds);
    /// <summary>
    /// Add specified tasks by array ids to the project with projectId. (Update tasks by setting project ids as param projectId)
    /// </summary>
    /// <param name="projectId">The id of project</param>
    /// <param name="token">Cancellation token</param>
    /// <param name="taskIds">the array of task ids</param>
    System.Threading.Tasks.Task AddTasksAsync(Guid projectId, CancellationToken token = default, params Guid[] taskIds);

    /// <summary>
    /// Filter projects by various fields
    /// </summary>
    /// <param name="filter">the body of filter: status, priority, name, start and end dates</param>
    /// <param name="sort">The options of sorting: flags by status, by priority, etc.</param>
    /// <param name="token">Cancellation token</param>
    Task<IEnumerable<ProjectViewDto<Guid>>> FilterAsync(ProjectsFilterDto? filter, ProjectsSortDto? sort,
        CancellationToken token = default);
}