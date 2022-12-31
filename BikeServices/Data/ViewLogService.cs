using System.Text.Json;

namespace BikeServices.Data;

public class ViewLogService
{
    private static void SaveAll(List<ViewLogItems> logItems)
    {
        string appDataDirectoryPath = Utils.GetAppDirectoryPath();
        string viewLogFilePath = Utils.GetViewLogFilePath();

        if (!Directory.Exists(appDataDirectoryPath))
        {
            Directory.CreateDirectory(appDataDirectoryPath);
        }

        var json = JsonSerializer.Serialize(logItems);
        File.WriteAllText(viewLogFilePath, json);
    }

    public static List<ViewLogItems> GetAll()
    {
        string viewLogFilePath = Utils.GetViewLogFilePath();
        if (!File.Exists(viewLogFilePath))
        {
            return new List<ViewLogItems>();
        }

        var json = File.ReadAllText(viewLogFilePath);

        return JsonSerializer.Deserialize<List<ViewLogItems>>(json);
    }

    public static List<ViewLogItems> Create(Guid userId, string itemName, DateTime actionDate, string actionPerformed)
    {
        List<ViewLogItems> logItems = GetAll();
        if (logItems == null)
        {
            throw new Exception("item not found.");
        }
        logItems.Add(new ViewLogItems
        {
            ItemName = itemName,
            ActionDate = actionDate,
            ActionPerformed = actionPerformed,
            ActionPerformer = userId
        });
        SaveAll(logItems);
        return logItems;
    }
}

