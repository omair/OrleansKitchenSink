using Grains;
using Moq;
using Orleans.Runtime;
using Orleans.TestKit;
using Shouldly;
using System;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace Grain.UnitTests
{
    public class HelloGrainTests : TestKitBase
    {
        [Fact]
        public async Task Silo_ShouldReturn_New_Grain()
        {
            var state = new GreetingState();
            var mockState = new Mock<IPersistentState<GreetingState>>();
            mockState.SetupGet(o => o.State).Returns(state);

            var mockMapper = new Mock<IAttributeToFactoryMapper<PersistentStateAttribute>>();
            mockMapper.Setup(o => o.GetFactory(It.IsAny<ParameterInfo>(), It.IsAny<PersistentStateAttribute>())).Returns(context => mockState.Object);

            Silo.AddService(mockMapper.Object);

            var grain = await Silo.CreateGrainAsync<HelloGrain>(Guid.NewGuid());

            grain.ShouldNotBeNull();
        }


        [Fact]
        public async Task SayHello_Should_Return_Greeting()
        {
            var state = new GreetingState();
            var mockState = new Mock<IPersistentState<GreetingState>>();
            mockState.SetupGet(o => o.State).Returns(state);

            var mockMapper = new Mock<IAttributeToFactoryMapper<PersistentStateAttribute>>();
            mockMapper.Setup(o => o.GetFactory(It.IsAny<ParameterInfo>(), It.IsAny<PersistentStateAttribute>())).Returns(context => mockState.Object);

            Silo.AddService(mockMapper.Object);

            var grain = await Silo.CreateGrainAsync<HelloGrain>(Guid.NewGuid());
            var from = Guid.NewGuid();
            var greeting = await grain.SayHello(from.ToString());
            greeting.ShouldBe($"Got Hello, World from {from.ToString()}");
        }


        [Fact]
        public async Task GetAllGreetings_Should_Return_All_Greetings()
        {
            var state = new GreetingState();
            var mockState = new Mock<IPersistentState<GreetingState>>();
            mockState.SetupGet(o => o.State).Returns(state);

            var mockMapper = new Mock<IAttributeToFactoryMapper<PersistentStateAttribute>>();
            mockMapper.Setup(o => o.GetFactory(It.IsAny<ParameterInfo>(), It.IsAny<PersistentStateAttribute>())).Returns(context => mockState.Object);

            Silo.AddService(mockMapper.Object);

            var grain = await Silo.CreateGrainAsync<HelloGrain>(Guid.NewGuid());
            var from = Guid.NewGuid();
            _ = await grain.SayHello(from.ToString());
            _ = await grain.SayHello(from.ToString());
            _ = await grain.SayHello(from.ToString());

            var allGreetings = await grain.GetAllGreetings();
            allGreetings.Count.ShouldBe(3);
        }

    }
}
