using Microsoft.Net.Http.Headers;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Mall.Test.IntegrationTests
{
    public class Tests : IClassFixture<TestingWebApplicationFactory<Mall.Startup>>
    {
        private readonly TestingWebApplicationFactory<Mall.Startup> _factory;

        public Tests(TestingWebApplicationFactory<Mall.Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task CreateStore_SentWrongModel_ReturnsFieldIsRequired()
        {
            // Arrange
            var client = _factory.CreateClient();

            var initResponse = await client.GetAsync("/Stores/Create");
            var antiForgeryValues = await AntiForgeryTokenExtractor.ExtractAntiForgeryValues(initResponse);

            var postRequest = new HttpRequestMessage(HttpMethod.Post, "/Stores/Create");
            postRequest.Headers.Add("Cookie", new CookieHeaderValue(AntiForgeryTokenExtractor.AntiForgeryCookieName, antiForgeryValues.cookieValue).ToString());

            var formModel = new Dictionary<string, string>
            {
                { AntiForgeryTokenExtractor.AntiForgeryFieldName, antiForgeryValues.fieldValue },
                { "RoomId", "" },
                { "StoreName", "TESTStoreName" },
                { "StoreDescription", "TESTStoreDescription" }
            };

            postRequest.Content = new FormUrlEncodedContent(formModel);

            // Act
            var response = await client.SendAsync(postRequest);

            // Assert
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            Assert.Contains("The Room number field is required.", responseString);
        }

        [Fact]
        public async Task CreateRoom_SentWrongModel_ReturnsFieldIsRequired()
        {
            // Arrange
            var client = _factory.CreateClient();

            var initResponse = await client.GetAsync("/Rooms/Create");
            var antiForgeryValues = await AntiForgeryTokenExtractor.ExtractAntiForgeryValues(initResponse);

            var postRequest = new HttpRequestMessage(HttpMethod.Post, "/Rooms/Create");
            postRequest.Headers.Add("Cookie", new CookieHeaderValue(AntiForgeryTokenExtractor.AntiForgeryCookieName, antiForgeryValues.cookieValue).ToString());

            var formModel = new Dictionary<string, string>
            {
                { AntiForgeryTokenExtractor.AntiForgeryFieldName, antiForgeryValues.fieldValue },
                { "Rent", "" }
            };

            postRequest.Content = new FormUrlEncodedContent(formModel);

            // Act
            var response = await client.SendAsync(postRequest);

            // Assert
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            Assert.Contains("The Rent field is required.", responseString);
        }

        [Fact]
        public async Task CreateRoom_SentWrongModel_ReturnsFieldMustBeNumber()
        {
            // Arrange
            var client = _factory.CreateClient();

            var initResponse = await client.GetAsync("/Rooms/Create");
            var antiForgeryValues = await AntiForgeryTokenExtractor.ExtractAntiForgeryValues(initResponse);

            var postRequest = new HttpRequestMessage(HttpMethod.Post, "/Rooms/Create");
            postRequest.Headers.Add("Cookie", new CookieHeaderValue(AntiForgeryTokenExtractor.AntiForgeryCookieName, antiForgeryValues.cookieValue).ToString());

            var formModel = new Dictionary<string, string>
            {
                { AntiForgeryTokenExtractor.AntiForgeryFieldName, antiForgeryValues.fieldValue },
                { "Rent", "TEST" }
            };

            postRequest.Content = new FormUrlEncodedContent(formModel);

            // Act
            var response = await client.SendAsync(postRequest);

            // Assert
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            Assert.Contains("The field Rent must be a number.", responseString);
        }

        [Fact]
        public async Task CreateCategory_WhenPOSTExecuted_ReturnsToIndexViewWithCreatedEmployee()
        {
            // Arrange
            var client = _factory.CreateClient();

            var initResponse = await client.GetAsync("/Category/Create");
            var antiForgeryValues = await AntiForgeryTokenExtractor.ExtractAntiForgeryValues(initResponse);

            var postRequest = new HttpRequestMessage(HttpMethod.Post, "/Category/Create");
            postRequest.Headers.Add("Cookie", new CookieHeaderValue(AntiForgeryTokenExtractor.AntiForgeryCookieName, antiForgeryValues.cookieValue).ToString());

            var formModel = new Dictionary<string, string>
            {
                { AntiForgeryTokenExtractor.AntiForgeryFieldName, antiForgeryValues.fieldValue },
                { "CategoryName", "TESTCategoryName" },
                { "CategoryDescription", "TESTCategoryDescription" }
            };

            postRequest.Content = new FormUrlEncodedContent(formModel);

            // Act
            var response = await client.SendAsync(postRequest);

            // Assert
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            Assert.Contains("TESTCategoryName", responseString);
            Assert.Contains("TESTCategoryDescription", responseString);
        }
    }
}
