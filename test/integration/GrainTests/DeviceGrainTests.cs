using GrainInterfaces;
using Grains.IntegrationTests.Cluster;
using Orleans.TestingHost;
using Shouldly;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Grains.IntegrationTests
{
    [Collection(ClusterCollection.Name)]
    public class DeviceGrainTests
    {
        private readonly TestCluster _cluster;
        private readonly ClusterFixture _fixture;

        public DeviceGrainTests(ClusterFixture fixture)
        {
            _cluster = fixture.Cluster;
            _fixture = fixture;
        }

        [Fact]
        public async Task Ping_Should_Return_CountOfPings()
        {
            var grain = _cluster.GrainFactory.GetGrain<IDeviceGrain>(Guid.NewGuid().ToString());
            var pingTime = DateTime.Now;
            var count = await grain.Ping(pingTime);
            count.ShouldBe(1);
            count = await grain.Ping(pingTime.AddSeconds(1));
            count.ShouldBe(2);
        }

        [Fact]
        public async Task Ping_Should_Ignore_Older_Pings()
        {
            var grain = _cluster.GrainFactory.GetGrain<IDeviceGrain>(Guid.NewGuid().ToString());
            var pingTime = DateTime.Now;
            var count = await grain.Ping(pingTime);
            count.ShouldBe(1);
            count = await grain.Ping(pingTime.AddSeconds(-1));
            count.ShouldBe(1);
        }

        [Fact]
        public async Task Events_Should_Ignore_All_Pings()
        {
            var grain = _cluster.GrainFactory.GetGrain<IDeviceGrain>(Guid.NewGuid().ToString());
            var pingTime = DateTime.Now;
            (await grain.RetrieveConfirmedEvents()).Count.ShouldBe(0);
            await grain.Ping(pingTime);
            (await grain.RetrieveConfirmedEvents()).Count.ShouldBe(1);
            await grain.Ping(pingTime.AddSeconds(1));
            (await grain.RetrieveConfirmedEvents()).Count.ShouldBe(2);
        }


        [Fact]
        public async Task Status_Should_Be_Offline_When_No_Ping_Is_Sent()
        {
            var grain = _cluster.GrainFactory.GetGrain<IDeviceGrain>(Guid.NewGuid().ToString());
            (await grain.Status()).ShouldBe(DeviceStatus.Offline);
        }

        [Fact]
        public async Task Status_Should_Change_To_Online_Once_Ping_Is_Sent()
        {
            var grain = _cluster.GrainFactory.GetGrain<IDeviceGrain>(Guid.NewGuid().ToString());
            var pingTime = DateTime.Now;
            (await grain.Status()).ShouldBe(DeviceStatus.Offline);
            await grain.Ping(pingTime);
            (await grain.Status()).ShouldBe(DeviceStatus.Online);
        }

        [Fact]
        public async Task Reminder_Should_Not_Be_NullAsync()
        {
            var grain = _cluster.GrainFactory.GetGrain<IDeviceGrain>(Guid.NewGuid().ToString());
            var reminder = _fixture.GetReminder(grain, ("UpdateDeviceStatusAsync"));
            reminder.ShouldBeNull();
            await grain.Ping(DateTime.Now);
            reminder = _fixture.GetReminder(grain, ("UpdateDeviceStatusAsync"));
            reminder.ShouldNotBeNull();
        }

    }
}
