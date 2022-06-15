using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Domain.Enums;
using Domain.Interfaces.Dtos;

namespace Domain.Dtos.Projects;

public record ProjectUpdateDto<TId> : IDto<TId>
{
    [JsonIgnore]
    public TId Id { get; set; }
    public string Name { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime CompletionDate { get; set; }
    public ProjectStatus Status { get; set; }
    public int Priority { get; set; }
}