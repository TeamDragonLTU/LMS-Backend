namespace Domain.Models.Exceptions;

public class SaveFailureException : Exception
{
    public string Title { get; }
    public int StatusCode { get; }

    public SaveFailureException(string message, string title = "Could not save")
        : base(message)
    {
        Title = title;
        StatusCode = 500;
    }
}
