using AutoMapper;
using Domain.Models.Entities;
using LMS.Shared.DTOs.ActivityDtos;
using LMS.Shared.DTOs.ActivityTypeDto;
using LMS.Shared.DTOs.AuthDtos;
using LMS.Shared.DTOs.CourseDtos;
using LMS.Shared.DTOs.Module;


namespace LMS.Infractructure.Data;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<UserRegistrationDto, ApplicationUser>();

        CreateMap<Course, CourseDto>();
        CreateMap<Course, CreateCourseDto>().ReverseMap();
        CreateMap<Course, UpdateCourseDto>().ReverseMap();



        CreateMap<Module, ModuleDto>();
        CreateMap<CreateModuleDto, Module>();
        CreateMap<UpdateModuleDto, Module>();


        CreateMap<Activity, ActivityDto>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.ActivityType.Name));
        CreateMap<ActivityType, ActivityTypeDto>();
        CreateMap<CreateActivityDto, Activity>();
        CreateMap<UpdateActivityDto, Activity>();
    }
}
