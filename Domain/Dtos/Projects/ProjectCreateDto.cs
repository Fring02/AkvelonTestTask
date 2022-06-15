using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Domain.Enums;
using Domain.Interfaces.Dtos;

namespace Domain.Dtos.Projects;

public record ProjectCreateDto<TId> : IDto<TId>
{
    [JsonIgnore]
    public TId Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public DateTime StartDate { get; set; }
    [Required]
    public DateTime CompletionDate { get; set; }
    [JsonIgnore]
    public ProjectStatus Status => ProjectStatus.NotStarted;
    [Required]
    public int Priority { get; set; }

}