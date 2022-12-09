using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinanceAPI.NET.Infrastructure.Connectivity
{
    public class TimeSyncState
    {
        public string ApiName { get; set; }
        public SemaphoreSlim Semaphore { get; set; }
        public DateTime LastSyncTime { get; set; }
        public TimeSpan TimeOffset { get; set; }

        public TimeSyncState(string apiName)
        {
            ApiName = apiName;
            Semaphore = new SemaphoreSlim(1, 1);
        }
    }
    public class TimeSyncInfo
    {
        private ILogger logger;

        public bool SyncTime { get; }
        public TimeSpan RecalculationInterval { get; }
        public TimeSyncState TimeSyncState { get; }

        public TimeSyncInfo(ILogger logger, bool syncTime, TimeSpan recalculationInterval, TimeSyncState timeSyncState)
        {
            this.logger = logger;
            SyncTime = syncTime;
            RecalculationInterval = recalculationInterval;
            TimeSyncState = timeSyncState;
        }

        public void UpdateTimeOffset(TimeSpan offset)
        {
            TimeSyncState.LastSyncTime = DateTime.UtcNow;
            if (offset.TotalMilliseconds> 0 && offset.TotalMilliseconds < 500)
            {
                logger.LogInformation($"{TimeSyncState.ApiName} Time offset whitin limits, setting offset to 0 ");
                TimeSyncState.TimeOffset = TimeSpan.Zero;
            } else
            {
                logger.LogInformation($"{TimeSyncState.ApiName} Time offset set to {Math.Round(offset.TotalMilliseconds)}ms");
                TimeSyncState.TimeOffset = offset;
            }
        }
    }
}
