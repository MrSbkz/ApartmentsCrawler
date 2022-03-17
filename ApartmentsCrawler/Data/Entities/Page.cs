namespace ApartmentsCrawler.Data.Entities;

public class Page
{
    public Guid Id { get; set; }
    
    public string Area { get; set; }

    public string Street { get; set; }

    public float PropertySize { get; set; }

    public int RoomsCount { get; set; }

    public float Price { get; set; }
    
    public string City { get; set; }

    public string Source { get; set; }

    public string Link { get; set; }
}