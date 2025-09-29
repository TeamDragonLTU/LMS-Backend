namespace LMS.Shared.DTOs.ApplicationUserDtos
{
    public record ApplicationUserDto(
        string Email,
        string UserName,
        string Role
    )
    {
        public ApplicationUserDto() : this(string.Empty, string.Empty, string.Empty) { }
    }
}
