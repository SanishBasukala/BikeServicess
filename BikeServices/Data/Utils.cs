using System.Security.Cryptography;

namespace BikeServices.Data;

public static class Utils
{
    private const char _segmentDelimiter = ':';
    // Way of encrypting password
    public static string HashSecret(string input)
    {
        var saltSize = 16;
        var iterations = 100_000;
        var keySize = 32;
        HashAlgorithmName algorithm = HashAlgorithmName.SHA256;
        byte[] salt = RandomNumberGenerator.GetBytes(saltSize);
        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(input, salt, iterations, algorithm, keySize);

        return string.Join(
            _segmentDelimiter,
            Convert.ToHexString(hash),
            Convert.ToHexString(salt),
            iterations,
            algorithm
        );
    }
    // Comparing password with json file
    public static bool VerifyHash(string input, string hashString)
    {
        string[] segments = hashString.Split(_segmentDelimiter);
        byte[] hash = Convert.FromHexString(segments[0]);
        byte[] salt = Convert.FromHexString(segments[1]);
        int iterations = int.Parse(segments[2]);
        HashAlgorithmName algorithm = new(segments[3]);
        byte[] inputHash = Rfc2898DeriveBytes.Pbkdf2(
            input,
            salt,
            iterations,
            algorithm,
            hash.Length
        );
        return CryptographicOperations.FixedTimeEquals(inputHash, hash);
    }
    // Create a folder for all json files.
    public static string GetAppDirectoryPath()
    {
        return Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "BikeService"
        );
    }

    // Create json file for users.
    public static string GetAppUsersFilePath()
    {
        return Path.Combine(GetAppDirectoryPath(), "users.json");
    }
    // Create json file for items.
    public static string GetItemsFilePath()
    {
        return Path.Combine(GetAppDirectoryPath(), "items.json");
    }

    // Create json file for log of added and deleted items.
    public static string GetViewLogFilePath()
    {
        return Path.Combine(GetAppDirectoryPath(), "ViewLog.json");
    }
    // Create json file for requested items.
    public static string GetInventoryLogFilePath()
    {
        return Path.Combine(GetAppDirectoryPath(), "InventoryLog.json");
    }
}