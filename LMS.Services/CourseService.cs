using AutoMapper;
using Domain.Contracts.Repositories;
using LMS.Shared.DTOs.CourseDtos;
using Service.Contracts;

namespace LMS.Services
{
    public class CourseService : ICourseService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CourseService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IEnumerable<CourseDto>> GetAllCoursesAsync()
        {
            var courses = await _unitOfWork.Courses.GetCoursesAsync();
            return _mapper.Map<IEnumerable<CourseDto>>(courses);
        }
    }
}
