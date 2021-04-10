using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GrainInterfaces
{
    public interface IDeviceGrain : Orleans.IGrainWithStringKey
    {
        Task<long> Ping(DateTime timestamp);

        Task<IReadOnlyList<IDeviceEvent>> RetrieveConfirmedEvents();
        Task<string> Status();
    }

    public interface IDeviceEvent
    {
        DateTimeOffset Timestamp { get; }
    }




}
