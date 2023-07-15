using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FindMe.Models
{
    public class User : BaseEntity
    {
        [EmailAddress(ErrorMessage = "Incorrect address")]
        public string? Email { get; set; }

        [Column(TypeName = "binary(16)")]
        public byte[] Salt { get; set; }
        [Column(TypeName = "nvarchar(150)")]
        public string Hash { get; set; }

        [Phone(ErrorMessage = "Invalid phone number")]
        public string? PhoneNumber { get; set; }

        public DateTime CreatedDate { get; set; }
        public bool Active { get; set; } = true;

        public ICollection<Person>? People { get; set; }
    }
}
