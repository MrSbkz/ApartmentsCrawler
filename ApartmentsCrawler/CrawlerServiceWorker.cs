using System.ComponentModel;
using ApartmentsCrawler.Models;
using ApartmentsCrawler.Services.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ApartmentsCrawler;

public class CrawlerServiceWorker : IHostedService
{
    private Timer _timer;
    private readonly ILogger<CrawlerServiceWorker> _logger;
    private readonly ICrawlerService _crawlerService;
    private readonly Provider _configuration;

    public CrawlerServiceWorker(ILogger<CrawlerServiceWorker> logger,
        ICrawlerService crawlerService,
        IConfiguration configuration)
    {
        _logger = logger;
        _crawlerService = crawlerService;
        _configuration = new Provider();
        configuration.GetSection("Provider").Bind(_configuration);
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation(nameof(CrawlerServiceWorker) + " is working");
        _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromHours(_configuration.TimeSpan));

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation(nameof(CrawlerServiceWorker) + " is stopping");

        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    private void DoWork(object state)
    {
        _crawlerService.RunAsync();
    }
}