namespace FindMe.Models
{
    public record HashedPassword(byte[] salt, string hashed);
}
