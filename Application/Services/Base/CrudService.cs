using AutoMapper;
using AutoMapper.QueryableExtensions;
using Data.Contexts;
using Domain.Entities.Base;
using Domain.Interfaces.Dtos;
using Domain.Interfaces.Services;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Base;

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
    public virtual async Task<TCreateDto> CreateAsync(TCreateDto createEntity, CancellationToken token) 
    {
        var entity = _mapper.Map<TEntity>(createEntity);
        _context.Set<TEntity>().Add(entity);
        await _context.SaveChangesAsync(token);
        createEntity.Id = entity.Id;
        return createEntity;
    }

    public abstract System.Threading.Tasks.Task UpdateAsync(TUpdateDto updateEntity, CancellationToken token);

    public async Task<IEnumerable<TViewDto>> GetAllAsync(CancellationToken token)
        => await _context.Set<TEntity>().AsNoTracking().ProjectTo<TViewDto>(_mapper.ConfigurationProvider).ToListAsync(token);

    public Task<TDetailedViewDto?> GetByIdAsync(Guid id, CancellationToken token)
        => _context.Set<TEntity>().AsNoTracking().Where(e => e.Id == id)
            .ProjectTo<TDetailedViewDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(token);

    public async System.Threading.Tasks.Task DeleteByIdAsync(Guid id, CancellationToken token)
    {
        var entity = await _context.Set<TEntity>().FindAsync(id);
        if (entity == null) throw new ArgumentException($"Entity with id {id} is not found");
        _context.Set<TEntity>().Remove(entity);
        await _context.SaveChangesAsync(token);
    }
}