using Microsoft.AspNetCore.Mvc.Testing;
using Shouldly;
using System;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xunit;

namespace Api.IntegrationTests
{
    public class DeviceControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public DeviceControllerTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }


        [Fact]
        public async Task Ping_Should_Return_Count()
        {
            var deviceId = Guid.NewGuid();
            var pingUrl = $"/device/{deviceId}/ping";
            var client = _factory.CreateClient();
            var response = await client.GetAsync(pingUrl);
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            var body = await response.Content.ReadAsStringAsync();
            body.ShouldBe("1");
        }

        [Fact]
        public async Task Events_Should_Return_All_Pings()
        {
            var deviceId = Guid.NewGuid();
            var pingUrl = $"/device/{deviceId}/ping";
            var eventsUrl = $"/device/{deviceId}/events";
            var client = _factory.CreateClient();

            var eventsResponseBeforePing = await client.GetAsync(eventsUrl);
            eventsResponseBeforePing.StatusCode.ShouldBe(HttpStatusCode.OK);
            var body = await eventsResponseBeforePing.Content.ReadAsStringAsync();
            body.ShouldBe("[]");

            var pingResponse = await client.GetAsync(pingUrl);
            pingResponse.StatusCode.ShouldBe(HttpStatusCode.OK);
            pingResponse = await client.GetAsync(pingUrl);
            pingResponse.StatusCode.ShouldBe(HttpStatusCode.OK);

            var eventsResponseAfterPing = await client.GetAsync(eventsUrl);
            eventsResponseAfterPing.StatusCode.ShouldBe(HttpStatusCode.OK);
            body = await eventsResponseAfterPing.Content.ReadAsStringAsync();
            body.ShouldNotBeNullOrEmpty();
            Regex.Matches(body, "timestamp").Count.ShouldBe(2);

        }


        [Fact]
        public async Task Status_Should_Return_Device_Status()
        {
            var deviceId = Guid.NewGuid();
            var pingUrl = $"/device/{deviceId}/ping";
            var statusUrl = $"/device/{deviceId}/status";
            var client = _factory.CreateClient();

            var eventsResponseBeforePing = await client.GetAsync(statusUrl);
            eventsResponseBeforePing.StatusCode.ShouldBe(HttpStatusCode.OK);
            var body = await eventsResponseBeforePing.Content.ReadAsStringAsync();
            body.ShouldBe("Offline");

            var pingResponse = await client.GetAsync(pingUrl);
            pingResponse.StatusCode.ShouldBe(HttpStatusCode.OK);
            var eventsResponseAfterPing = await client.GetAsync(statusUrl);
            eventsResponseAfterPing.StatusCode.ShouldBe(HttpStatusCode.OK);
            body = await eventsResponseAfterPing.Content.ReadAsStringAsync();
            body.ShouldBe("Online");


        }

    }
}
