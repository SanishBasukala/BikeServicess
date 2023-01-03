using System.Text.Json;


namespace BikeServices.Data;
public static class InventoryService
{
    // Save all items in json file.
    private static void SaveAll(Guid userId, List<Items> items)
    {
        string appDataDirectoryPath = Utils.GetAppDirectoryPath();
        string itemsFilePath = Utils.GetItemsFilePath();

        if (!Directory.Exists(appDataDirectoryPath))
        {
            Directory.CreateDirectory(appDataDirectoryPath);
        }

        var json = JsonSerializer.Serialize(items);
        File.WriteAllText(itemsFilePath, json);
    }

    // Get all items from json file.
    public static List<Items> GetAll()
    {
        string itemsFilePath = Utils.GetItemsFilePath();
        if (!File.Exists(itemsFilePath))
        {
            return new List<Items>();
        }

        var json = File.ReadAllText(itemsFilePath);

        return JsonSerializer.Deserialize<List<Items>>(json);
    }

    // Create new items for adding in the inventory.
    public static List<Items> Create(Guid userId, string itemName, int quanity)
    {
        List<Items> items = GetAll();
        items.Add(new Items
        {
            ItemName = itemName,
            Quanity = quanity,
            LastTakenOut = "Not taken out yet"
        });
        SaveAll(userId, items);
        return items;
    }

    // Delete items from the inventory
    public static List<Items> Delete(Guid userId, Guid id)
    {
        List<Items> items = GetAll();
        Items item = items.FirstOrDefault(x => x.Id == id);

        if (item == null)
        {
            throw new Exception("item not found.");
        }

        items.Remove(item);
        SaveAll(userId, items);
        return items;
    }

    // Delete items from json file
    public static void DeleteByUserId(Guid userId)
    {
        string itemsFilePath = Utils.GetItemsFilePath();
        if (File.Exists(itemsFilePath))
        {
            File.Delete(itemsFilePath);
        }
    }

    // Update items after editing.
    public static List<Items> Update(Guid userId, Guid id, string itemName, int quantity)
    {
        List<Items> items = GetAll();
        Items itemToUpdate = items.FirstOrDefault(x => x.Id == id);

        if (itemToUpdate == null)
        {
            throw new Exception("item not found.");
        }
        itemToUpdate.ItemName = itemName;
        itemToUpdate.Quanity = quantity;
        SaveAll(userId, items);
        return items;
    }

    // Change the date and quantity when items are requested.
    public static List<Items> ChangeOnRequest(Guid userId, Guid id, string itemName, string lastTakenOut, int quantityTaken)
    {
        List<Items> items = GetAll();
        Items itemToUpdate = items.FirstOrDefault(x => x.Id == id);
        itemToUpdate.ItemName = itemName;
        itemToUpdate.LastTakenOut = lastTakenOut;
        itemToUpdate.Quanity -= quantityTaken;
        SaveAll(userId, items);
        return items;
    }
}

