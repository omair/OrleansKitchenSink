using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GrainInterfaces
{
    public interface IDeviceGrain : Orleans.IGrainWithStringKey
    {
        Task<long> Ping(DateTime timestamp);

        Task<IReadOnlyList<IDeviceEvent>> RetrieveConfirmedEvents();
        Task<DeviceStatus> Status();
    }

    public interface IDeviceEvent
    {
        DateTimeOffset Timestamp { get; }
        String EventType { get; }
    }

    public enum DeviceStatus
    {
        Offline,
        Idle,
        Online
    }


}
