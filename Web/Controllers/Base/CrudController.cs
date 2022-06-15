using Domain.Interfaces.Dtos;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.Base;

/// <summary>
/// Generic CRUD controller for managing data
/// </summary>
/// <typeparam name="TCreateDto">The entity to be created</typeparam>
/// <typeparam name="TUpdateDto">The entity to be edited</typeparam>
/// <typeparam name="TViewDto">The entity to be viewed</typeparam>
/// <typeparam name="TDetailedViewDto">The entity to be viewed more properly</typeparam>
/// <typeparam name="TService">The service which provides all the CRUD operations</typeparam>
[ApiController]
public abstract class CrudController<TCreateDto, TUpdateDto, TViewDto, TDetailedViewDto, TService> : ControllerBase
where TService : ICrudService<Guid, TCreateDto, TUpdateDto, TViewDto, TDetailedViewDto>
where TUpdateDto : class, IDto<Guid>
where TViewDto : class, IDto<Guid>
where TDetailedViewDto : IDto<Guid>
where TCreateDto : IDto<Guid>
{
    protected readonly TService _service;
    protected CrudController(TService service)
    {
        _service = service;
    }

    /// <summary>
    /// Get an entity with id
    /// </summary>
    /// <param name="id">Id of an entity</param>
    /// <param name="token">Cancellation token</param>
    /// <response code="200">Returns entity by id</response>
    /// <response code="404">When entity with given id is not found</response>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(Guid id, CancellationToken token)
    {
        var entity = await _service.GetByIdAsync(id, token);
        return entity == null ? NotFound() : Ok(entity);
    }

    /// <summary>
    /// Returns all the entities
    /// </summary>
    /// <param name="token">Cancellation token</param>
    /// <returns>The list of entities</returns>
    [HttpGet]
    public virtual async Task<IActionResult> GetAllAsync(CancellationToken token)
        => Ok(await _service.GetAllAsync(token));

    /// <summary>
    /// Create an entity
    /// </summary>
    /// <param name="dto">The body of entity to be created</param>
    /// <param name="token">Cancellation token</param>
    /// <response code="201">Returns the created entity</response>
    /// <response code="400">When the body of entity is invalid</response>
    /// <response code="500">When it's impossible to create entity due to server error</response>
    [HttpPost]
    public virtual async Task<IActionResult> CreateAsync([FromBody] TCreateDto dto, CancellationToken token)
    {
        try
        {
            var createdEntity = await _service.CreateAsync(dto, token);
            return Created(Request.Path, new { body = createdEntity, id = createdEntity.Id });
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
    /// Update entity by id
    /// </summary>
    /// <param name="id">The id of entity to be updated</param>
    /// <param name="dto">The entity fields that need to be updated</param>
    /// <param name="token">Cancellation token</param>
    /// <response code="200">The entity is updated successfully</response>
    /// <response code="400">When the body of entity or id is invalid; when entity cannot be found</response>
    /// <response code="500">When it's impossible to update entity due to server error</response>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] TUpdateDto dto, CancellationToken token)
    {
        try
        {
            dto.Id = id;
            await _service.UpdateAsync(dto, token);
            return Ok("Updated");
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
    /// Delete entity by id
    /// </summary>
    /// <param name="id">The id of entity to be updated</param>
    /// <param name="token">Cancellation token</param>
    /// <response code="200">The entity is deleted successfully</response>
    /// <response code="400">When the entity is not found or id is invalid</response>
    /// <response code="500">When it's impossible to delete entity due to server error</response>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(Guid id, CancellationToken token)
    {
        try
        {
            await _service.DeleteByIdAsync(id, token);
            return Ok("Deleted");
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
}