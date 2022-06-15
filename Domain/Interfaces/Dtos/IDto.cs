namespace Domain.Interfaces.Dtos;

//Is used for defining DTOs which have entities' Ids
public interface IDto<TId>
{
    TId Id { get; set; }
}