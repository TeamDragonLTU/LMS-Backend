using AutoMapper;
using Domain.Contracts.Repositories;
using Domain.Models.Entities;
using Domain.Models.Exceptions;
using LMS.Shared.DTOs.ApplicationUserDtos;
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

        public async Task<CourseDto> GetCourseAsync(Guid id)
        {
            var course = await _unitOfWork.Courses.GetCourseAsync(id);
            if (course == null)
                throw new CourseNotFoundException(id);

            var courseDto = _mapper.Map<CourseDto>(course);

            return courseDto;
        }

        public async Task PutCourseAsync(Guid id, UpdateCourseDto dto)
        {
            var course = await _unitOfWork.Courses.GetCourseAsync(id, trackChanges: true);

            if (course == null)
                throw new CourseNotFoundException(id);

            _mapper.Map(dto, course);

            try
            {
                await _unitOfWork.CompleteAsync();
            }
            catch (Exception)
            {
                if (!await _unitOfWork.Courses.AnyAsync(c => c.Id == id))
                    throw new SaveFailureException("Could not save the course");
                else
                    throw;
            }
        }

        public async Task<CourseDto> PostCourseAsync(CreateCourseDto dto)
        {
            var course = _mapper.Map<Course>(dto);

            _unitOfWork.Courses.Create(course);
            await _unitOfWork.CompleteAsync();

            var courseDto = _mapper.Map<CourseDto>(course);

            return courseDto;
        }

        public async Task DeleteCourseAsync(Guid id)
        {
            var course = await _unitOfWork.Courses.GetCourseAsync(id);

            if (course == null)
                throw new CourseNotFoundException(id);

            _unitOfWork.Courses.Delete(course);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<CourseDto> GetCourseWithModulesAndActivitiesAsync(string userId)
        {
            var course = await _unitOfWork.Courses.GetCourseWithModulesAndActivitiesAsync(userId);
            if (course == null)
                throw new NotFoundException("No course found for user");

            return _mapper.Map<CourseDto>(course);
        }

        public async Task<IEnumerable<ApplicationUserDto>> GetCourseParticipantsByUserIdAsync(string userId)
        {
            var participantsWithRoles = await _unitOfWork.Courses.GetCourseParticipantsByUserIdAsync(userId);
            var dtos = participantsWithRoles.Select(tuple => new ApplicationUserDto(
                tuple.User.Email ?? string.Empty,
                tuple.User.UserName ?? string.Empty,
                tuple.Role ?? string.Empty
            ));
            return dtos;
        }
    }
}