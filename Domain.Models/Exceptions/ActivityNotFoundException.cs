using Domain.Models.Exceptions;

public class ActivityNotFoundException : NotFoundException
{
    public ActivityNotFoundException(Guid activityId)
        : base($"Activity with id '{activityId}' was not found.", "Activity not found")
    {
    }
}