using ApartmentsCrawler.Services.Contracts;

namespace ApartmentsCrawler.Services;

public class CrawlerService : ICrawlerService
{
    public Task Run()
    {
        return Task.CompletedTask;
    }
}