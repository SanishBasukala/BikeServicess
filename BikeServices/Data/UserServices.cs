﻿using System.Text.Json;

namespace BikeServices.Data;

public static class UsersService
{
    public const string SeedUsername = "admin";
    public const string SeedPassword = "admin";

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

    public static List<User> Create(Guid userId, string username, string password)
    {
        List<User> users = GetAll();
        bool usernameExists = users.Any(x => x.Username == username);

        if (usernameExists)
        {
            throw new Exception("Username already exists.");
        }

        users.Add(
            new User
            {
                Username = username,
                CreatedBy = userId
            }
        );
        SaveAll(users);
        return users;
    }
    public static User GetById(Guid id)
    {
        List<User> users = GetAll();
        return users.FirstOrDefault(x => x.Id == id);
    }

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

    public static User Login(string username, string password)
    {
        var loginErrorMessage = "Invalid username or password.";
        List<User> users = GetAll();
        User user = users.FirstOrDefault(x => x.Username == username);

        if (user == null)
        {
            throw new Exception(loginErrorMessage);
        }


        return user;
    }

}
