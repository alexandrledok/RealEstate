using System.Net.Http.Json;
using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using RealEstate.Api;

namespace RealEstate.Tests
{
    public class IntegrationPropertiesTests: IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public IntegrationPropertiesTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Post_Returns400_WhenInvalidBody()
        {
            var payload = new { price = -5 };

            var response = await _client.PostAsJsonAsync("/api/v1/properties", payload);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task GetAll_WorksWithFilters()
        {
            var url = "/api/v1/properties?type=house&min_price=10&max_price=500";

            var response = await _client.GetAsync(url);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
