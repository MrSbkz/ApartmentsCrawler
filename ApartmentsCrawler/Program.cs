using ApartmentsCrawler.Data;
using ApartmentsCrawler.Services;
using ApartmentsCrawler.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ApartmentsCrawler;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.AddDbContext<ApartmentsCrawlerContext>(opt =>
                    opt.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=ApartmentsCrawlerDb;Integrated Security=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"));
                services.AddHostedService<CrawlerServiceWorker>();
                services.AddTransient<ICrawlerService, CrawlerService>();
            });
}