namespace FindMe.Models
{
    public record SaltHashedPassword(byte[] salt, string hash);
}
