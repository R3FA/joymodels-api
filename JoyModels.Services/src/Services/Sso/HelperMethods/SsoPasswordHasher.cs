using Microsoft.AspNetCore.Identity;

namespace JoyModels.Services.Services.Sso;

public static class SsoPasswordHasher
{
    public static string Hash<T>(T request, string password) where T : class
        => Hasher<T>.Instance.HashPassword(request, password);

    public static PasswordVerificationResult Verify<T>(T request, string hashedPassword, string providedPassword)
        where T : class
        => Hasher<T>.Instance.VerifyHashedPassword(request, hashedPassword, providedPassword);

    private static class Hasher<T> where T : class
    {
        internal static readonly PasswordHasher<T> Instance = new();
    }
}