namespace LMS.Shared.DTOs.ApplicationUserDtos
{
    public record ApplicationUserDto(
        string Email,
        string UserName
    )
    {
        public ApplicationUserDto() : this(string.Empty, string.Empty) { }
    }
}
