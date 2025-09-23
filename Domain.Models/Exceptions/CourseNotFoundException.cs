using Domain.Models.Exceptions;

public class CourseNotFoundException : NotFoundException
{
    public CourseNotFoundException(Guid courseId)
        : base($"Course with id '{courseId}' was not found.", "Course not found")
    {
    }
}