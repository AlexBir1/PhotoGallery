
namespace PhotoGalleryAPI.Shared.Password
{
    public static class PassHandler
    {
        public static bool VerifyPassword(string password, string passwordHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }

        public static string CreatePasswordHash(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt(4));
        }
    }
}
