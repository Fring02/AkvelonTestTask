using AutoMapper;
using Domain.Dtos.Projects;
using Domain.Entities;

namespace Application.Mappings;

public class ProjectsProfile : Profile
{
    public ProjectsProfile()
    {
        CreateMap<ProjectCreateDto<Guid>, Project>();
        CreateMap<Project, ProjectViewDto<Guid>>().ForMember(p => p.Status, o => o.MapFrom(pr => pr.GetStatusName(pr.Status)))
            .ForMember(p => p.StartDate, o => o.MapFrom(pr => pr.StartDate.ToLongDateString()))
            .ForMember(p => p.CompletionDate, o => o.MapFrom(pr => pr.CompletionDate.ToLongDateString()));
        CreateMap<Project, ProjectDetailedViewDto<Guid>>().ForMember(p => p.Status, o => o.MapFrom(pr => pr.GetStatusName(pr.Status)))
            .ForMember(p => p.StartDate, o => o.MapFrom(pr => pr.StartDate.ToLongDateString()))
            .ForMember(p => p.CompletionDate, o => o.MapFrom(pr => pr.CompletionDate.ToLongDateString()));
    }
}