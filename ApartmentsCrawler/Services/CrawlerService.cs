using ApartmentsCrawler.Data;
using ApartmentsCrawler.Data.Entities;
using ApartmentsCrawler.Services.Contracts;

namespace ApartmentsCrawler.Services;

public class CrawlerService : ICrawlerService
{
    private const string OfferAttribute =
        "vc_row wpb_row vc_row-fluid vc_row-o-columns-middle vc_row-o-equal-height vc_row-flex";

    private readonly INowodworskiService _nowodworskiService;
    private readonly ApartmentsCrawlerContext _context;

    public CrawlerService(INowodworskiService nowodworskiService, ApartmentsCrawlerContext context)
    {
        _nowodworskiService = nowodworskiService;
        _context = context;
    }

    public async Task RunAsync()
    {
        var offers = new List<Offer>();
        offers.AddRange(await _nowodworskiService.GetOffersAsync());
    }
}