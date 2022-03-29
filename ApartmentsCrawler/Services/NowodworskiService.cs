using System.Net;
using ApartmentsCrawler.Data.Entities;
using ApartmentsCrawler.Helpers;
using ApartmentsCrawler.Models;
using ApartmentsCrawler.Services.Contracts;
using HtmlAgilityPack;
using Microsoft.Extensions.Configuration;

namespace ApartmentsCrawler.Services;

public class NowodworskiService : INowodworskiService
{
    private const string OfferAttribute =
        "vc_row wpb_row vc_row-fluid vc_row-o-columns-middle vc_row-o-equal-height vc_row-flex";

    private readonly HtmlWeb _crawler;
    private readonly NowodworskiUrlModel _urlModel;

    public NowodworskiService(IConfiguration configuration)
    {
        _urlModel = new NowodworskiUrlModel();
        configuration.GetSection("NowodworskiUrlModel").Bind(_urlModel);
        _crawler = new HtmlWeb();
        _crawler.PreRequest = delegate(HttpWebRequest request)
        {
            request.Timeout = 4000; // number of milliseconds
            return true;
        };
    }

    public async Task<IList<Offer>> GetOffersAsync()
    {
        var offers = new List<Offer>();
        var pageCount = 1;

        for (int i = 1; i <= pageCount; i++)
        {
            var url = $"{_urlModel.SearchUrl}{i}";
            var document = await _crawler.LoadFromWebAsync(url);

            if (pageCount == 1)
            {
                var pageLimitSection =
                    document.DocumentNode.Descendants().FirstOrDefault(x => x.Id.Equals("page_limit"));
                int.TryParse(pageLimitSection!.InnerText, out pageCount);
            }

            var apartmentSections = document.DocumentNode.Descendants().Where(node =>
                node.Attributes.Any(a => a.Value.Equals(OfferAttribute)));

            foreach (var apartmentSection in apartmentSections)
            {
                var apartmentElements = apartmentSection.Descendants().ToList();

                var pathUrl = NowodworskiHelper.GetUrlPath(apartmentElements);
                if (pathUrl != null && !NowodworskiHelper.IsUrlValid(pathUrl) || pathUrl == null)
                    continue;

                var location = NowodworskiHelper.GetLocation(apartmentElements);
                var roomCount = NowodworskiHelper.GetRoomCount(apartmentElements);
                var price = NowodworskiHelper.GetPrice(apartmentElements);
                var isReserved = NowodworskiHelper.IsReserved(apartmentElements);

                if (location != null && roomCount != 0 && price != 0)
                {
                    var commaIndex = location!.IndexOf(",", StringComparison.Ordinal);
                    var city = commaIndex != -1 ? location[..commaIndex] : location;

                    offers.Add(new Offer
                    {
                        Source = _urlModel.Source,
                        Link = pathUrl.Contains("https://")
                            ? pathUrl
                            : _urlModel.SourceUrl + pathUrl,
                        Location = location,
                        City = city,
                        RoomCount = roomCount,
                        Price = price,
                        IsReserved = isReserved,
                    });
                }
            }
        }

        return offers;
    }
}