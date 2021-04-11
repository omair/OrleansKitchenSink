using GrainInterfaces;
using Microsoft.Extensions.Options;
using Orleans;
using Orleans.EventSourcing;
using Orleans.Providers;
using Orleans.Runtime;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Grains
{

    [StorageProvider(ProviderName = "DeviceGrainStorage")]
    [LogConsistencyProvider(ProviderName = "LogStorage")]
    public class DeviceGrain : JournaledGrain<DeviceState, IDeviceEvent>, IDeviceGrain, IRemindable
    {

        private readonly DeviceGrainOptions _deviceGrainOptions;

        public DeviceGrain(IOptions<DeviceGrainOptions> options)
        {
            _deviceGrainOptions = options.Value;
        }


        #region Reminders

        public override async Task OnActivateAsync()
        {
            await RegisterOrUpdateReminder(nameof(UpdateDeviceStatusAsync), TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1));
            await base.OnActivateAsync();
        }

        private Task UpdateDeviceStatusAsync(TickStatus status)
        {

            var diffFromLastPing = status.CurrentTickTime - State.LastPingTimestamp;
            if (diffFromLastPing > _deviceGrainOptions.OfflineTimeout)
            {
                return UpdateDeviceStatusToOffline();
            }
            else if (diffFromLastPing > _deviceGrainOptions.IdleTimeout)
            {
                return UpdateDeviceStatusToIdle();
            }
            return Task.CompletedTask;

        }

        private Task UpdateDeviceStatusToIdle()
        {
            if (State.Status == DeviceStatus.Idle) return Task.CompletedTask;
            RaiseEvent(new DeviceIdleEvent(State.LastPingTimestamp));
            return ConfirmEvents();
        }

        private Task UpdateDeviceStatusToOffline()
        {
            if (State.Status == DeviceStatus.Offline) return Task.CompletedTask;
            RaiseEvent(new DeviceOfflineEvent(State.LastPingTimestamp));
            return ConfirmEvents();
        }

        public Task ReceiveReminder(string reminderName, TickStatus status)
        {
            if (reminderName == nameof(UpdateDeviceStatusAsync)) return UpdateDeviceStatusAsync(status);
            return Task.CompletedTask;
        }

        #endregion

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

        public Task<DeviceStatus> Status() => Task.FromResult(State.Status);

    }

    public class DeviceGrainOptions
    {
        public TimeSpan IdleTimeout { get; set; } = new TimeSpan(0, 1, 0);
        public TimeSpan OfflineTimeout { get; set; } = new TimeSpan(0, 2, 0);

    }

    [Serializable]
    public class DeviceState
    {

        public DateTimeOffset LastPingTimestamp { get; private set; }
        public DeviceStatus Status { get; private set; }
        public long PingCount { get; private set; }

        public DeviceState()
        {
            Status = DeviceStatus.Offline;
            LastPingTimestamp = DateTime.MinValue;
            PingCount = 0;
        }


        public void Apply(PingEvent e)
        {
            LastPingTimestamp = e.Timestamp;
            Status = DeviceStatus.Online;
            PingCount++;
        }

        public void Apply(DeviceIdleEvent e) => Status = DeviceStatus.Idle;


        public void Apply(DeviceOfflineEvent e) => Status = DeviceStatus.Offline;

    }


    public abstract class DeviceEvent : IDeviceEvent
    {
        public DeviceEvent(DateTimeOffset timestamp, String eventType)
        {
            Timestamp = timestamp;
            EventType = eventType;
        }

        public DateTimeOffset Timestamp { get; private set; }

        public string EventType { get; private set; }
    }


    public class PingEvent : DeviceEvent
    {
        public PingEvent(DateTimeOffset timestamp) : base(timestamp, nameof(PingEvent)) { }
    }

    public class DeviceIdleEvent : DeviceEvent
    {
        public DeviceIdleEvent(DateTimeOffset timestamp) : base(timestamp, nameof(DeviceIdleEvent)) { }
    }

    public class DeviceOfflineEvent : DeviceEvent
    {
        public DeviceOfflineEvent(DateTimeOffset timestamp) : base(timestamp, nameof(DeviceOfflineEvent)) { }
    }

}
