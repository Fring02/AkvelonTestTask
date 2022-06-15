using Domain.Dtos.Tasks;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Web.Controllers.Base;

namespace Web.Controllers.Tasks;

[Route("api/v1/[controller]")]
public class TasksController : CrudController<TaskCreateDto<Guid>, 
    TaskUpdateDto<Guid>, TaskViewDto<Guid>, TaskDetailedViewDto<Guid>, ITasksService>
{
    public TasksController(ITasksService service) : base(service)
    {
    }

    /// <summary>
    /// Returns all the tasks: or tasks with the specified Project Id in query param
    /// </summary>
    /// <param name="token">Cancellation token</param>
    /// <returns>The list of tasks</returns>
    public override async Task<IActionResult> GetAllAsync(CancellationToken token)
    {
        if (Guid.TryParse(Request.Query["projectId"], out Guid projectId))
            return Ok(await _service.GetByProjectAsync(projectId, token));
        
        return await base.GetAllAsync(token);
    }
}