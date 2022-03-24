namespace ApartmentsCrawler.Data.Entities;

public class Offer
{
    public Guid Id { get; set; }

    public string City { get; set; }

    public int RoomCount { get; set; }

    public float Price { get; set; }
    
    public string Location { get; set; }

    public string Source { get; set; }

    public string Link { get; set; }

    public bool IsReserved { get; set; }
}