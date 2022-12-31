﻿using System.Text.Json;

namespace BikeServices.Data;

public class RequestItemService
{
    public static void SaveAll(List<RequestItems> requestItems)
    {
        string appDataDirectoryPath = Utils.GetAppDirectoryPath();
        string requestItemFilePath = Utils.GetInventoryLogFilePath();

        if (!Directory.Exists(appDataDirectoryPath))
        {
            Directory.CreateDirectory(appDataDirectoryPath);
        }

        var json = JsonSerializer.Serialize(requestItems);
        File.WriteAllText(requestItemFilePath, json);
    }

    public static List<RequestItems> GetAll()
    {
        string requestItemFilePath = Utils.GetInventoryLogFilePath();
        if (!File.Exists(requestItemFilePath))
        {
            return new List<RequestItems>();
        }

        var json = File.ReadAllText(requestItemFilePath);

        return JsonSerializer.Deserialize<List<RequestItems>>(json);
    }

    public static List<RequestItems> Create(Guid userId, string itemName, int quantityTaken, string takenBy, DateTime dateTakenOut)
    {
        List<RequestItems> requestItems = GetAll();
        requestItems.Add(new RequestItems
        {
            ItemName = itemName,
            QuantityTaken = quantityTaken,
            ApprovedBy = userId,
            TakenBy = takenBy,
            DateTakenOut = dateTakenOut
        });
        SaveAll(requestItems);
        return requestItems;
    }
}