using GrainInterfaces;
using Orleans.EventSourcing;
using Orleans.Providers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Grains
{

    [StorageProvider(ProviderName = "DeviceGrainStorage")]
    [LogConsistencyProvider(ProviderName = "LogStorage")]
    public class DeviceGrain : JournaledGrain<DeviceState, IDeviceEvent>, IDeviceGrain
    {
        public async Task<long> Ping(DateTime timestamp)
        {
            if (timestamp > State.LastPingTimestamp)
            {
                RaiseEvent(new PingEvent(timestamp));
                await ConfirmEvents();
            }
            return State.PingCount;
        }

        public Task<IReadOnlyList<IDeviceEvent>> RetrieveConfirmedEvents()
        {
            return RetrieveConfirmedEvents(0, Version);
        }

        public Task<string> Status() => Task.FromResult(State.IsOnline ? "Online" : "Offline");

    }

    [Serializable]
    public class DeviceState
    {

        public DateTimeOffset LastPingTimestamp { get; private set; }
        public bool IsOnline { get; private set; }
        public long PingCount { get; private set; }

        public DeviceState()
        {
            IsOnline = false;
            LastPingTimestamp = DateTime.MinValue;
            PingCount = 0;
        }


        public void Apply(PingEvent e)
        {
            LastPingTimestamp = e.Timestamp;
            IsOnline = true;
            PingCount++;
        }
    }




    public class PingEvent : IDeviceEvent
    {
        public PingEvent(DateTime timestamp) => Timestamp = timestamp;

        public DateTimeOffset Timestamp { get; private set; }
    }

}
