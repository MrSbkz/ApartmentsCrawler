using ApartmentsCrawler.Data.Entities;

namespace ApartmentsCrawler.Services.Contracts;

public interface IOfferService
{
    Task<IList<Offer>> GetOffersAsync();
}