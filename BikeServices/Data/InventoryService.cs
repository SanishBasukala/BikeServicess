using System.Text.Json;


namespace BikeServices.Data;
public static class InventoryService
{
    private static void SaveAll(Guid userId, List<Items> todos)
    {
        string appDataDirectoryPath = Utils.GetAppDirectoryPath();
        string itemsFilePath = Utils.GetItemsFilePath(userId);

        if (!Directory.Exists(appDataDirectoryPath))
        {
            Directory.CreateDirectory(appDataDirectoryPath);
        }

        var json = JsonSerializer.Serialize(todos);
        File.WriteAllText(itemsFilePath, json);
    }

    public static List<Items> GetAll(Guid userId)
    {
        string itemsFilePath = Utils.GetItemsFilePath(userId);
        if (!File.Exists(itemsFilePath))
        {
            return new List<Items>();
        }

        var json = File.ReadAllText(itemsFilePath);

        return JsonSerializer.Deserialize<List<Items>>(json);
    }

    public static List<Items> Create(Guid userId, string taskName, int quanity, DateTime lastTakenOut)
    {


        List<Items> todos = GetAll(userId);
        todos.Add(new Items
        {
            ItemName = taskName,
            Quanity = quanity,
            LastTakenOut = lastTakenOut
        });
        SaveAll(userId, todos);
        return todos;
    }

    public static List<Items> Delete(Guid userId, Guid id)
    {
        List<Items> todos = GetAll(userId);
        Items todo = todos.FirstOrDefault(x => x.Id == id);

        if (todo == null)
        {
            throw new Exception("Todo not found.");
        }

        todos.Remove(todo);
        SaveAll(userId, todos);
        return todos;
    }

    public static void DeleteByUserId(Guid userId)
    {
        string itemsFilePath = Utils.GetItemsFilePath(userId);
        if (File.Exists(itemsFilePath))
        {
            File.Delete(itemsFilePath);
        }
    }

    public static List<Items> Update(Guid userId, Guid id, string taskName, int quantity, DateTime lastTakenOut)
    {
        List<Items> todos = GetAll(userId);
        Items todoToUpdate = todos.FirstOrDefault(x => x.Id == id);

        if (todoToUpdate == null)
        {
            throw new Exception("Todo not found.");
        }

        todoToUpdate.ItemName = taskName;
        todoToUpdate.Quanity = quantity;
        todoToUpdate.LastTakenOut = lastTakenOut;
        SaveAll(userId, todos);
        return todos;
    }
}

