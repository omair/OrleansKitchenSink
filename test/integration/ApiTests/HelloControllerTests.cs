using Microsoft.AspNetCore.Mvc.Testing;
using Shouldly;
using Xunit;

namespace Api.IntegrationTests
{
    public class HelloContollerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public HelloContollerTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }


        [Fact]
        public async System.Threading.Tasks.Task Get_Should_Return_All_GreetingsAsync()
        {
            var url = "/hello";
            var client = _factory.CreateClient();
            var response = await client.GetAsync(url);
            response.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);
            var body = await response.Content.ReadAsStringAsync();
            body.ShouldContain("Got Hello, World from");
        }
    }
}
