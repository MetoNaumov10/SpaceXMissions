using System.Security.Cryptography;

namespace Services
{
    public static class PasswordHasher
    {
        public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var rng = RandomNumberGenerator.Create();
            passwordSalt = new byte[16];
            rng.GetBytes(passwordSalt);


            using var pbkdf2 = new Rfc2898DeriveBytes(password, passwordSalt, 100_000, HashAlgorithmName.SHA256);
            passwordHash = pbkdf2.GetBytes(32); // 256-bit
        }


        public static bool VerifyPassword(string password, byte[] storedHash, byte[] storedSalt)
        {
            using var pbkdf2 = new Rfc2898DeriveBytes(password, storedSalt, 100_000, HashAlgorithmName.SHA256);
            var computed = pbkdf2.GetBytes(32);
            return CryptographicOperations.FixedTimeEquals(computed, storedHash);
        }
    }
}
