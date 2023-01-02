using System.Text.Json;

namespace BikeServices.Data;

public static class UserService
{
    // Default admin credentials.
    public const string SeedUsername = "admin";
    public const string SeedPassword = "admin";

    // Save all users.
    private static void SaveAll(List<User> users)
    {
        string appDataDirectoryPath = Utils.GetAppDirectoryPath();
        string appUsersFilePath = Utils.GetAppUsersFilePath();

        if (!Directory.Exists(appDataDirectoryPath))
        {
            Directory.CreateDirectory(appDataDirectoryPath);
        }

        var json = JsonSerializer.Serialize(users);
        File.WriteAllText(appUsersFilePath, json);
    }

    // Get all users of the system.
    public static List<User> GetAll()
    {
        string appUsersFilePath = Utils.GetAppUsersFilePath();
        if (!File.Exists(appUsersFilePath))
        {
            return new List<User>();
        }

        var json = File.ReadAllText(appUsersFilePath);

        return JsonSerializer.Deserialize<List<User>>(json);
    }

    // Create new users.
    public static List<User> Create(Guid userId, string username, string password, Role role)
    {
        List<User> users = GetAll();
        bool usernameExists = users.Any(x => x.Username == username);

        int userRole = users.Count(x => x.Role == Role.Admin);

        if (userRole >= 2)
        {
            throw new Exception("Error");
        }

        if (usernameExists)
        {
            throw new Exception("Username already exists.");
        }

        users.Add(
            new User
            {
                Username = username,
                PasswordHash = Utils.HashSecret(password),
                Role = role,
                CreatedBy = userId
            }
        );
        SaveAll(users);
        return users;
    }

    // Creating default admin if not already available.
    public static void SeedUsers()
    {
        var seedUser = GetAll().FirstOrDefault(x => x.Role == Role.Admin);

        if (seedUser == null)
        {
            Create(Guid.Empty, SeedUsername, SeedPassword, Role.Admin);
        }
    }

    // Get users by their unique id.
    public static User GetById(Guid id)
    {
        List<User> users = GetAll();
        return users.FirstOrDefault(x => x.Id == id);
    }

    // Delete users.
    public static List<User> Delete(Guid id)
    {
        List<User> users = GetAll();
        User user = users.FirstOrDefault(x => x.Id == id);

        if (user == null)
        {
            throw new Exception("User not found.");
        }

        InventoryService.DeleteByUserId(id);
        users.Remove(user);
        SaveAll(users);

        return users;
    }

    // Check credentials when loggin in the system.
    public static User Login(string username, string password)
    {
        var loginErrorMessage = "Invalid username or password.";
        List<User> users = GetAll();

        User user = users.FirstOrDefault(x => x.Username == username);

        if (user == null)
        {
            throw new Exception(loginErrorMessage);
        }

        bool passwordIsValid = Utils.VerifyHash(password, user.PasswordHash);

        if (!passwordIsValid)
        {
            throw new Exception(loginErrorMessage);

        }
        return user;
    }
}
