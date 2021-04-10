using Grains;
using Shouldly;
using System;
using Xunit;

namespace Grain.UnitTests
{
    public class DeviceStateTests
    {

        [Fact]
        public void PingCount_Should_Be_Zero_When_New_State_Is_Created()
        {
            var s = new DeviceState();
            s.PingCount.ShouldBe(0);
        }

        [Fact]
        public void LastPingTimestamp_Should_Be_MinDate_When_New_State_Is_Created()
        {
            var s = new DeviceState();
            s.LastPingTimestamp.ShouldBe(DateTime.MinValue);
        }

        [Fact]
        public void IsOnline_Should_Be_False_When_New_State_Is_Created()
        {
            var s = new DeviceState();
            s.IsOnline.ShouldBe(false);
        }


        [Fact]
        public void PingCount_Should_Increment_By_One_When_PingEvent_Is_Applied()
        {
            var s = new DeviceState();
            var pingTime = DateTime.Now;
            
            s.Apply(new PingEvent(pingTime));
            s.PingCount.ShouldBe(1);

            s.Apply(new PingEvent(pingTime.AddSeconds(1)));
            s.PingCount.ShouldBe(2);

            s.Apply(new PingEvent(pingTime.AddSeconds(-1)));
            s.PingCount.ShouldBe(3);

            s.Apply(new PingEvent(pingTime));
            s.PingCount.ShouldBe(4);
        }


        [Fact]
        public void LastPingTime_Should_Match_Time_Of_Last_Ping()
        {
            var s = new DeviceState();
            var pingTime = DateTime.Now;

            s.Apply(new PingEvent(pingTime));
            s.LastPingTimestamp.ShouldBe(pingTime);

            s.Apply(new PingEvent(pingTime));
            s.LastPingTimestamp.ShouldBe(pingTime);

            s.Apply(new PingEvent(pingTime.AddSeconds(-1)));
            s.LastPingTimestamp.ShouldBe(pingTime.AddSeconds(-1));

            s.Apply(new PingEvent(pingTime.AddSeconds(1)));
            s.LastPingTimestamp.ShouldBe(pingTime.AddSeconds(1));
        }

        [Fact]
        public void Ping_Should_Change_Online_Status_To_True()
        {
            var s = new DeviceState();
            var pingTime = DateTime.Now;

            s.Apply(new PingEvent(pingTime));
            s.IsOnline.ShouldBe(true);

        }

    }
}
