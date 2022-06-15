using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Domain.Entities;
using Domain.Interfaces.Dtos;

namespace Domain.Dtos.Tasks;

public record TaskUpdateDto<TId> : IDto<TId>
{
    [JsonIgnore]
    public TId Id { get; set; }
    [MaxLength(255)]
    public string? Name { get; set; }
    public string? Description { get; set; }
    public TaskStatus? Status { get; set; }
    public int? Priority { get; set; }
    public Guid? ProjectId { get; set; } = Guid.Empty;
}