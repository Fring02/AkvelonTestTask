namespace Domain.Interfaces.Dtos;

public interface IDto<TId>
{
    TId Id { get; set; }
}