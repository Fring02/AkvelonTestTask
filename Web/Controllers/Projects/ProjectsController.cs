using Domain.Dtos.Projects;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Web.Controllers.Base;

namespace Web.Controllers.Projects;

[Route("api/v1/[controller]")]
public class ProjectsController : CrudController<ProjectCreateDto<Guid>, 
    ProjectUpdateDto<Guid>, ProjectViewDto<Guid>, ProjectDetailedViewDto<Guid>, IProjectsService>
{
    public ProjectsController(IProjectsService service) : base(service)
    {
    }

    /// <summary>
    /// Add tasks (array of task ids) to project with given id (update tasks' project ids)
    /// </summary>
    /// <param name="id">Id of an project</param>
    /// <param name="taskIds">Array of task ids</param>
    /// <param name="token">Cancellation token</param>
    /// <response code="200">Tasks are added</response>
    /// <response code="400">When tasks with given ids are not present or when project with id is not present</response>
    [HttpPost("{id}/tasks")]
    public async Task<IActionResult> AddTasksAsync(Guid id, [FromBody] Guid[] taskIds, CancellationToken token)
    {
        try
        {
            await _service.AddTasksAsync(projectId: id, token: token, taskIds);
            return Ok($"Added tasks to project {id}");
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
    /// <summary>
    /// Remove tasks (array of task ids) from project with given id (update tasks' project ids)
    /// </summary>
    /// <param name="id">Id of an project</param>
    /// <param name="taskIds">Array of task ids</param>
    /// <param name="token">Cancellation token</param>
    /// <response code="200">Tasks are removed (project ids are null)</response>
    /// <response code="400">When tasks with given ids are not present or when project with id is not present</response>
    [HttpDelete("{id}/tasks")]
    public async Task<IActionResult> DeleteTasksAsync(Guid id, [FromBody] Guid[] taskIds, CancellationToken token)
    {
        try
        {
            await _service.DeleteTasksAsync(projectId: id, token: token, taskIds);
            return Ok($"Removed tasks from project {id}");
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    /// <summary>
    /// Filter and sort projects by various fields (dates, priority, status, etc.)
    /// </summary>
    /// <param name="token">Cancellation token</param>
    /// <param name="filter">Filter body from query params</param>
    /// <param name="sort">Sort flags from query params</param>
    /// <returns>The list of filtered and sorted projects</returns>
    [HttpGet("filter")]
    public async Task<IActionResult> FilterAsync(CancellationToken token, [FromQuery] ProjectsFilterDto filter,
        [FromQuery] ProjectsSortDto sort)
        => Ok(await _service.FilterAsync(filter, sort, token));
}