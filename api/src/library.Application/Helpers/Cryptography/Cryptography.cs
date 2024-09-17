namespace library.Application.Helpers.Cryptography;

public class Cryptography : ICryptography
{
    public bool VerifyHash(string password, string storedHash)
    {
        return BCrypt.Net.BCrypt.Verify(password, storedHash);
    }

    public string Hash(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }
}
