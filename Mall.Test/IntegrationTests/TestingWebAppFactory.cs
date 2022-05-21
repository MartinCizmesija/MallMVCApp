using System;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Mall.Test
{
    namespace IntegrationTests
    {
        #region snippet1
        public class TestingWebApplicationFactory<TStartup>
            : WebApplicationFactory<TStartup> where TStartup : class
        {
            protected override void ConfigureWebHost(IWebHostBuilder builder)
            {
                builder.ConfigureServices(services =>
                {
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType ==
                            typeof(DbContextOptions<DbContext>));

                    services.Remove(descriptor);

                    services.AddDbContext<DbContext>(options =>
                    {
                        options.UseInMemoryDatabase("InMemoryDbForTesting");
                    });

                    services.AddAntiforgery(t =>
                    {
                        t.Cookie.Name = AntiForgeryTokenExtractor.AntiForgeryCookieName;
                        t.FormFieldName = AntiForgeryTokenExtractor.AntiForgeryFieldName;
                    });

                    var sp = services.BuildServiceProvider();

                    using (var scope = sp.CreateScope())
                    {
                        var scopedServices = scope.ServiceProvider;
                        var db = scopedServices.GetRequiredService<DbContext>();
                        var logger = scopedServices
                            .GetRequiredService<ILogger<TestingWebApplicationFactory<TStartup>>>();

                        db.Database.EnsureCreated();
                    }
                });
            }
        }
        #endregion
    }
}
