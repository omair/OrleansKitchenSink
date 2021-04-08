using GrainInterfaces;
using Orleans.TestingHost;
using System;
using System.Threading.Tasks;
using Xunit;
using Shouldly;

namespace GrainTests
{
    [Collection(ClusterCollection.Name)]
    public class HelloGrainTests
    {
        private readonly TestCluster _cluster;
        public HelloGrainTests(ClusterFixture fixture)
        {
            _cluster = fixture.Cluster;
        }

        [Fact]
        public async Task SaysHello_Should_Return_Greeting()
        {
            var helloGrain = _cluster.GrainFactory.GetGrain<IHelloGrain>(Guid.NewGuid());
            var from = Guid.NewGuid().ToString();
            var greeting = await helloGrain.SayHello(from);
            greeting.ShouldBe($"Got Hello, World from {from}");
        }

        [Fact]
        public async Task GetAllGreetings_Should_Return_All_Greetings_Sent_To_Grain()
        {
            var helloGrain = _cluster.GrainFactory.GetGrain<IHelloGrain>(Guid.NewGuid());
            _ = await helloGrain.SayHello(Guid.NewGuid().ToString());
            _ = await helloGrain.SayHello(Guid.NewGuid().ToString());
            _ = await helloGrain.SayHello(Guid.NewGuid().ToString());
            var greetings = await helloGrain.GetAllGreetings();
            greetings.ShouldNotBeNull();
            greetings.Count.ShouldBe(3);
        }

        [Fact]
        public async Task Graing_Should_Only_Return_Its_Greetings()
        {
            var firstHelloGrain = _cluster.GrainFactory.GetGrain<IHelloGrain>(Guid.NewGuid());
            _ = await firstHelloGrain.SayHello(Guid.NewGuid().ToString());
            _ = await firstHelloGrain.SayHello(Guid.NewGuid().ToString());
            _ = await firstHelloGrain.SayHello(Guid.NewGuid().ToString());
            var greetingsFromFistGrain = await firstHelloGrain.GetAllGreetings();

            var secondHelloGrain = _cluster.GrainFactory.GetGrain<IHelloGrain>(Guid.NewGuid());
            _ = await secondHelloGrain.SayHello(Guid.NewGuid().ToString());
            var greetingsFromSecond = await secondHelloGrain.GetAllGreetings();

            greetingsFromFistGrain.ShouldNotBeNull();
            greetingsFromFistGrain.Count.ShouldBe(3);

            greetingsFromSecond.ShouldNotBeNull();
            greetingsFromSecond.Count.ShouldBe(1);
        }


    }
}
