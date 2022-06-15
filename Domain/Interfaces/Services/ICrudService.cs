using Domain.Interfaces.Dtos;

namespace Domain.Interfaces.Services;

//Generic CRUD interface for services: Create/edit/view/delete entities
public interface ICrudService<in TId, TCreateDto, in TUpdateDto, TViewDto, TDetailedViewDto>
    where TUpdateDto : IDto<TId> where TViewDto : class where TDetailedViewDto : IDto<TId>
        where TCreateDto : IDto<TId>
{
    Task<TCreateDto> CreateAsync(TCreateDto createEntity, CancellationToken token = default);
    System.Threading.Tasks.Task UpdateAsync(TUpdateDto updateEntity, CancellationToken token = default);
    Task<IEnumerable<TViewDto>> GetAllAsync(CancellationToken token = default);
    Task<TDetailedViewDto?> GetByIdAsync(TId id, CancellationToken token = default);
    System.Threading.Tasks.Task DeleteByIdAsync(TId id, CancellationToken token = default);
}