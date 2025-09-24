using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Domain.Contracts.Repositories;
using Domain.Models.Entities;
using Domain.Models.Exceptions;
using LMS.Shared.DTOs.CourseDtos;
using Service.Contracts;
using System.Data;

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
            var changes = await SaveChangesAsync();
            if (changes <= 0)
                throw new SaveFailureException("Could not save the course");

        }

        public async Task<CourseDto> PostCourseAsync(CreateCourseDto dto)
        {
            var course = _mapper.Map<Course>(dto);
            _unitOfWork.Courses.Create(course);
            var changes = await SaveChangesAsync();
            if (changes <= 0)
                throw new SaveFailureException("Could not save the course");
            var courseDto = _mapper.Map<CourseDto>(course);
            return courseDto;
        }

        public async Task DeleteCourseAsync(Guid id)
        {
            var course = await _unitOfWork.Courses.GetCourseAsync(id);

            if (course == null)
                throw new CourseNotFoundException(id);

            _unitOfWork.Courses.Delete(course);
            var changes = await SaveChangesAsync();
            if (changes <= 0)
                throw new SaveFailureException("Could not delete the course");
        }

        // Helper för att få antal ändringar från SaveChangesAsync
        private async Task<int> SaveChangesAsync()
        {
            var contextProp = _unitOfWork.GetType().GetField("_context", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (contextProp?.GetValue(_unitOfWork) is DbContext context)
                return await context.SaveChangesAsync();
            await _unitOfWork.CompleteAsync(); // fallback, men returnerar inget
            return 1; // antag att det lyckades om vi inte kan få context
        }

    }
}
