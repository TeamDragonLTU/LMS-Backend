namespace LMS.Shared.DTOs.ActivityTypeDto
{
    public record ActivityTypeDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
