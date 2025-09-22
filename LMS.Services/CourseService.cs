using AutoMapper;
using Domain.Contracts.Repositories;
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
            return _mapper.Map<IEnumerable<CourseDto>>(await _unitOfWork.Courses.GetCoursesAsync());
        }

        public async Task<CourseDto> GetCourseAsync(Guid id)
        {
            var course = await _unitOfWork.Courses.GetCourseAsync(id);
            if (course == null)
                throw new NotFoundException("Can't find the course");

            var courseDto = _mapper.Map<CourseDto>(course);

            return courseDto;
        }

        public async Task PutCourseAsync(Guid id, UpdateCourseDto dto)
        {
            var course = await _unitOfWork.Courses.GetCourseAsync(id, trackChanges: true);

            if (course == null)
                throw new NotFoundException("Course could not be found");

            _mapper.Map(dto, course);

            Console.WriteLine(course.Name);

            try
            {
                await _unitOfWork.CompleteAsync();
            }
            catch (Exception)
            {
                if (!await _unitOfWork.Courses.AnyCourseAsync(id))
                    throw new Exception("Could not save the course");
                else
                    throw;
            }

            //DbUpdateConcurrencyException ??

        }

    }
}
