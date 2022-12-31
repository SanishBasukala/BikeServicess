namespace BikeServices.Data;

public class RequestItems
{
    public string ItemName { get; set; }
    public int QuantityTaken { get; set; }
    public Guid ApprovedBy { get; set; }
    public string TakenBy { get; set; }
    public DateTime DateTakenOut { get; set; }

}

