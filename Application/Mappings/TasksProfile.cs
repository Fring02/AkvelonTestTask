using AutoMapper;
using Domain.Dtos.Tasks;
namespace Application.Mappings;

//Automapper Profile which maps task entity to DTos and vice versa
public class TasksProfile : Profile
{
    public TasksProfile()
    {
        CreateMap<TaskCreateDto<Guid>, Task>();
        CreateMap<Task, TaskViewDto<Guid>>().ForMember(d => d.Status, 
            opts => opts.MapFrom(t => t.GetStatusName(t.Status)));
        CreateMap<Task, TaskDetailedViewDto<Guid>>().ForMember(d => d.Status, 
            opts => opts.MapFrom(t => t.GetStatusName(t.Status)));
    }
}