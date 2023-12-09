using FitiChanBot.Settings;

namespace FitiChanBot
{
    public class BackgroundMonitor
    {
        public bool IsActive { get; private set; }
        private readonly MessageManager _msgManager;
        private readonly TimeSpan _delay;
        private readonly CancellationTokenSource delaysCancel;
        public BackgroundMonitor(MessageManager msgManager, FitiSettings fitiSettings)
        {
            _msgManager = msgManager;
            _delay = fitiSettings.MonitoringDelay;
            IsActive = false;
            delaysCancel = new CancellationTokenSource();
        }

        private async Task MonitoringLoop(CancellationToken delayCancel)
        {
            while (IsActive == true)
            {
                _msgManager.UpdateShortList(_delay);

                if (!_msgManager.IsShortMessagesListEmpty)
                    await _msgManager.SendShortListAsync();
                //    await SmallMonitoringLoop(delayCancel);

                try { await Task.Delay(_delay, delayCancel); }
                catch (TaskCanceledException) { break; }
            }
        }

        //private async Task SmallMonitoringLoop(CancellationToken delayCancel)
        //{
        //    while (!_msgManager.IsShortMessagesListEmpty)
        //    {
        //        await _msgManager.SendShortListAsync();
        //        try { await Task.Delay(_delaySetting.SmallDelay, delayCancel); }
        //        catch (TaskCanceledException) { break; }
        //    }
        //}

        public async Task StartAsync()
        {
            if (IsActive == true) throw new Exception("The monitoring service is already running. Stop it before starting it again");
            IsActive = true;
            await MonitoringLoop(delaysCancel.Token);
        }
        public void Stop()
        {
            IsActive = false;
            delaysCancel.Cancel();
        }
    }
}
