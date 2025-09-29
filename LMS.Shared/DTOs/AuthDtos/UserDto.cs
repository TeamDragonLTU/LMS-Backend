using System;

namespace LMS.Shared.DTOs.AuthDtos
{
    public class UserDto
    {
        public string Id { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public Guid? CourseId { get; set; }
        public List<string> Roles { get; set; } = new();
    }
}
