using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Domain.Interfaces.Dtos;

namespace Domain.Dtos.Tasks;

public record TaskCreateDto<TId> : IDto<TId>
{
    [JsonIgnore]
    public TId Id { get; set; }
    [MaxLength(255), Required]
    public string Name { get; set; }
    public string? Description { get; set; }
    public int Priority { get; set; }
    [Required]
    public Guid ProjectId { get; set; }
}