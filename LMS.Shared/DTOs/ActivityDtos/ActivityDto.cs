namespace LMS.Shared.DTOs.ActivityDtos
{
    public record ActivityDto
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public DateTime StartTime { get; init; }
        public DateTime EndTime { get; init; }
        public string Description { get; init; } = string.Empty;
        public string Type { get; init; } = string.Empty;
    }
}
