using ApartmentsCrawler.Data;
using ApartmentsCrawler.Data.Entities;
using ApartmentsCrawler.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace ApartmentsCrawler.Services;

public class DbService : IDbService
{
    private readonly ApartmentsCrawlerContext _context;

    public DbService(ApartmentsCrawlerContext context)
    {
        _context = context;
    }

    public async Task HandleOffersAsync(IList<Offer> offers)
    {
        var existingOffers = await _context.Offers.ToListAsync();

        await AddNewOffersToDbAsync(offers, existingOffers);
        DefineDeletedOffers(offers, existingOffers);

        foreach (var offer in offers)
        {
            var existingOffer =
                existingOffers.FirstOrDefault(x => x.Link.Equals(offer.Link, StringComparison.OrdinalIgnoreCase));

            if (existingOffer != null && IsOfferUpdated(offer, existingOffer))
            {
                // TODO: Notify users about updated offers by telegram bot

                existingOffer.IsReserved = offer.IsReserved;
                existingOffer.Price = offer.Price;

                _context.Offers.Update(existingOffer);
            }
        }

        await _context.SaveChangesAsync();
    }

    private async Task AddNewOffersToDbAsync(IList<Offer> offers, IList<Offer> existingOffers)
    {
        var newOffers = offers.Where(x =>
            !existingOffers.Any(e => e.Link.Equals(x.Link, StringComparison.OrdinalIgnoreCase))).ToList();

        // TODO: Notify users about new offers by telegram bot

        await _context.Offers.AddRangeAsync(newOffers);
    }

    private void DefineDeletedOffers(IList<Offer> offers, IList<Offer> existingOffers)
    {
        var deletedOffers = existingOffers.Where(x =>
            !offers.Any(e => e.Link.Equals(x.Link, StringComparison.OrdinalIgnoreCase))).ToList();

        _context.Offers.RemoveRange(deletedOffers);
    }

    private bool IsOfferUpdated(Offer offer, Offer existingOffer)
    {
        return offer.IsReserved != existingOffer.IsReserved || offer.Price != existingOffer.Price;
    }
}