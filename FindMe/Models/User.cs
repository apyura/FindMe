using System.ComponentModel.DataAnnotations;

namespace FindMe.Models
{
    public class User
    {
        public int Id { get; set; }

        [EmailAddress(ErrorMessage = "Incorrect address")]
        public string? Email { get; set; }

        public byte[] Salt { get; set; }
        public string Hash { get; set; }

        [Phone(ErrorMessage = "Invalid phone number")]
        public string? PhoneNumber { get; set; }

        public DateTime CreatedDate { get; set; }
        public bool Active { get; set; } = true;

        public ICollection<Person>? People { get; set; }
    }
}
