using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Mall.Test.IntegrationTests
{
    #region snippet1
    public class BasicTests
        : IClassFixture<WebApplicationFactory<Mall.Startup>>
    {
        private readonly WebApplicationFactory<Mall.Startup> _factory;

        public BasicTests(WebApplicationFactory<Mall.Startup> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("/")]
        [InlineData("/Stores")]
        [InlineData("/Products")]
        [InlineData("/Category")]
        [InlineData("/Rooms")]
        [InlineData("/Home/Privacy")]
        public async Task Get_EndpointsReturnSuccessAndCorrectContentType(string url)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal("text/html; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
        }
    }
    #endregion
}
