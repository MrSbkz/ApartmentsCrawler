using ApartmentsCrawler.Data.Entities;

namespace ApartmentsCrawler.Services.Contracts;

public interface IDbService
{
    Task HandleOffersAsync(IList<Offer> offers);
}