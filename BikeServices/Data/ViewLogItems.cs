namespace BikeServices.Data;

public class ViewLogItems
{
    public string ItemName { get; set; }
    public DateTime ActionDate { get; set; }
    public string ActionPerformed { get; set; }
    public Guid ActionPerformer { get; set; }
}

