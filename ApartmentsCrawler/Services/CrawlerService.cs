using ApartmentsCrawler.Data.Entities;
using ApartmentsCrawler.Services.Contracts;

namespace ApartmentsCrawler.Services;

public class CrawlerService : ICrawlerService
{
    private readonly INowodworskiService _nowodworskiService;
    private readonly IDbService _dbService;

    public CrawlerService(INowodworskiService nowodworskiService, IDbService dbService)
    {
        _nowodworskiService = nowodworskiService;
        _dbService = dbService;
    }

    public async Task RunAsync()
    {
        var offers = new List<Offer>();
        offers.AddRange(await _nowodworskiService.GetOffersAsync());

        await _dbService.HandleOffersAsync(offers);
    }
}