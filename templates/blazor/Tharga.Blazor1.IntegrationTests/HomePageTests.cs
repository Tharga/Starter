using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Tharga.Blazor1.IntegrationTests;

public class HomePageTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public HomePageTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Get_HomePage_ReturnsSuccessStatusCode()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Get_HomePage_ContainsExpectedContent()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/");
        var content = await response.Content.ReadAsStringAsync();

        Assert.Contains("blazor.web.js", content);
    }
}
