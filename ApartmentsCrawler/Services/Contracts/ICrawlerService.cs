﻿using ApartmentsCrawler.Data.Entities;

namespace ApartmentsCrawler.Services.Contracts;

public interface ICrawlerService
{
    Task RunAsync();
}