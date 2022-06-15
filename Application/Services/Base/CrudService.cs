using AutoMapper;
using AutoMapper.QueryableExtensions;
using Data.Contexts;
using Domain.Entities.Base;
using Domain.Interfaces.Dtos;
using Domain.Interfaces.Services;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Base;

/// <summary>
/// Generic CRUD base service which possesses the standard behavior for CRUD operations
/// </summary>
/// <typeparam name="TEntity">The entity in Domain layer</typeparam>
/// <typeparam name="TCreateDto">The DTO for creating an entity</typeparam>
/// <typeparam name="TUpdateDto">The DTO for editing an entity</typeparam>
/// <typeparam name="TViewDto">The DTO for viewing entity, used in lists</typeparam>
/// <typeparam name="TDetailedViewDto">The DTO for viewing entity in more detailed way</typeparam>
public abstract class CrudService<TEntity, TCreateDto, TUpdateDto, TViewDto, TDetailedViewDto>
    : ICrudService<Guid, TCreateDto, TUpdateDto, TViewDto, TDetailedViewDto> 
    where TEntity : BaseEntity<Guid> where TUpdateDto : IDto<Guid> where TViewDto : class
    where TDetailedViewDto : IDto<Guid>
    where TCreateDto : IDto<Guid>
{
    protected readonly ApplicationContext _context;
    protected readonly IMapper _mapper;
    protected CrudService(ApplicationContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    /// <summary>
    /// Create an entity by mapping DTO to entity and adding to context. After saving changes in database, gives the generated id back
    /// and DTO's body
    /// </summary>
    /// <param name="createEntity">The DTO for entity creation</param>
    /// <param name="token">Cancellation token</param>
    /// <returns>The created entity as DTO</returns>
    public virtual async Task<TCreateDto> CreateAsync(TCreateDto createEntity, CancellationToken token) 
    {
        var entity = _mapper.Map<TEntity>(createEntity);
        _context.Set<TEntity>().Add(entity);
        await _context.SaveChangesAsync(token);
        createEntity.Id = entity.Id;
        return createEntity;
    }

    public abstract System.Threading.Tasks.Task UpdateAsync(TUpdateDto updateEntity, CancellationToken token);
    /// <summary>
    /// Gets all the entities in the table and maps to view DTOs.
    /// </summary>
    /// <param name="token">Cancellation token</param>
    /// <returns>The list of view DTOs</returns>
    public async Task<IEnumerable<TViewDto>> GetAllAsync(CancellationToken token)
        => await _context.Set<TEntity>().AsNoTracking().ProjectTo<TViewDto>(_mapper.ConfigurationProvider).ToListAsync(token);

    /// <summary>
    /// Get an entity by id, map to more detailed DTO
    /// </summary>
    /// <param name="id">Id of an entity</param>
    /// <param name="token">Cancellation token</param>
    /// <returns>The entity as detailed view DTO</returns>
    public Task<TDetailedViewDto?> GetByIdAsync(Guid id, CancellationToken token)
        => _context.Set<TEntity>().AsNoTracking().Where(e => e.Id == id)
            .ProjectTo<TDetailedViewDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(token);

    /// <summary>
    /// Delete entity by id
    /// </summary>
    /// <param name="id">Id of an entity</param>
    /// <param name="token">Cancellation token</param>
    /// <exception cref="ArgumentException">If an entity is not found</exception>
    public async System.Threading.Tasks.Task DeleteByIdAsync(Guid id, CancellationToken token)
    {
        var entity = await _context.Set<TEntity>().FindAsync(id);
        if (entity == null) throw new ArgumentException($"Entity with id {id} is not found");
        _context.Set<TEntity>().Remove(entity);
        await _context.SaveChangesAsync(token);
    }
}