﻿namespace Mall.Test
{
    using System;
    using System.Linq;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Mall.Models;

    namespace RazorPagesProject.Tests
    {
        #region snippet1
        public class CustomWebApplicationFactory<TStartup>
            : WebApplicationFactory<TStartup> where TStartup : class
        {
            protected override void ConfigureWebHost(IWebHostBuilder builder)
            {
                builder.ConfigureServices(services =>
                {
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType ==
                            typeof(DbContextOptions<MallDbContext>));

                    services.Remove(descriptor);

                    services.AddDbContext<MallDbContext>(options =>
                    {
                        options.UseInMemoryDatabase("InMemoryDbForTesting");
                    });

                    var sp = services.BuildServiceProvider();

                    using (var scope = sp.CreateScope())
                    {
                        var scopedServices = scope.ServiceProvider;
                        var db = scopedServices.GetRequiredService<MallDbContext>();
                        var logger = scopedServices
                            .GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();

                        db.Database.EnsureCreated();
                    }
                });
            }
        }
        #endregion
    }
}
