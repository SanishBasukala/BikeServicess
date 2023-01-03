namespace BikeServices.Data;
public class Items
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string ItemName { get; set; }
    public int Quanity { get; set; }
    public string LastTakenOut { get; set; }
}
