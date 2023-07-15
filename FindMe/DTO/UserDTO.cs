using System.ComponentModel.DataAnnotations;

namespace FindMe.DTO
{
    public class UserDTO
    {
        [EmailAddress(ErrorMessage = "Incorrect address")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password not specified")]
        [StringLength(10, MinimumLength = 4, ErrorMessage = "The string length must be between 4 and 10 characters")]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string Password { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        [StringLength(10, MinimumLength = 4, ErrorMessage = "The string length must be between 4 and 10 characters")]
        public string? NewPassword { get; set; }
        [Phone(ErrorMessage = "Invalid phone number")]
        public string? PhoneNumber { get; set; }
    }
}
