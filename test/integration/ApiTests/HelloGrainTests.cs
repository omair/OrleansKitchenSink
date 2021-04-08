using Api;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using Shouldly;

namespace ApiTests
{
    public class OrealnsContollerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public OrealnsContollerTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        
        [Fact]
        public async System.Threading.Tasks.Task Get_Should_Return_All_GreetingsAsync()
        {
            var url = "/orleans";
            var client = _factory.CreateClient();
            var response = await client.GetAsync(url);
            response.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);
            var body = await response.Content.ReadAsStringAsync();
            body.ShouldContain("Got Hello, World from");
        }
    }
}
