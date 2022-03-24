using HtmlAgilityPack;

namespace ApartmentsCrawler.Helpers;

public static class NowodworskiHelper
{
    public static string? GetUrlPath(IEnumerable<HtmlNode> apartmentElements)
    {
        var hrefElement = apartmentElements!.FirstOrDefault(x => x.Name.Equals("a"));
        var apartmentUrlAttr =
            hrefElement?.Attributes.FirstOrDefault(attribute => attribute.Name.Equals("href"));

        return apartmentUrlAttr?.Value.Trim().ToLower();
    }

    public static bool IsUrlValid(string url)
    {
        return url != "#popup1";
    }

    public static string? GetLocation(IEnumerable<HtmlNode> apartmentElements)
    {
        var locationHeaderElement = apartmentElements!.FirstOrDefault(x => x.Name.Equals("h4"));
        return locationHeaderElement?.InnerText.Trim();
    }

    public static int GetRoomCount(IEnumerable<HtmlNode>? apartmentElements)
    {
        var roomCount = 0;
        var roomCountElements = apartmentElements!.Where(x =>
            x.Attributes.Any(a => a.Value.Equals("qodef-spec-item qodef-label-items-item"))).ToList();

        foreach (var roomCountElement in roomCountElements)
        {
            var labelElement = roomCountElement.Descendants()
                .FirstOrDefault(x => x.Attributes.Any(y =>
                    y.Value.Equals("qodef-label-text")));

            var labelInnerText = labelElement?.InnerText;
            if (labelInnerText != null && labelInnerText.Contains("No. of rooms"))
            {
                var roomCountValueElement = roomCountElement.Descendants()
                    .FirstOrDefault(x => x.Attributes.Any(y =>
                        y.Value.Equals("qodef-spec-item-value qodef-label-items-value")));

                var roomCountInnerText = roomCountValueElement?.InnerText.Trim();
                int.TryParse(roomCountInnerText, out roomCount);
                break;
            }
        }

        return roomCount;
    }

    public static int GetPrice(IEnumerable<HtmlNode>? apartmentElements)
    {
        var priceElements =
            apartmentElements!.Where(x => x.Name.Equals("h2")).ToList();

        var priceElement = priceElements.FirstOrDefault(x => x.OuterHtml.Contains("offer-price red-price")) ??
                           priceElements.FirstOrDefault(x => x.OuterHtml.Contains("offer-price "));


        var priceString = priceElement?.InnerText.Trim('P', 'L', 'N').Replace(" ", "");
        int.TryParse(priceString, out var price);

        return price;
    }

    public static bool IsReserved(IEnumerable<HtmlNode>? apartmentElements)
    {
        var bookedThumbnailElement =
            apartmentElements!.FirstOrDefault(x => x.Attributes.Any(y => y.Value.Equals("booked-thumbnail")));
        return bookedThumbnailElement != null;
    }
}