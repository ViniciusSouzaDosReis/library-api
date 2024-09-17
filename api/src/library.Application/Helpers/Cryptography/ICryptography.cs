namespace library.Application.Helpers.Cryptography;
public interface ICryptography
{
    bool VerifyHash(string password, string storedHash);
    string Hash(string password);
}
