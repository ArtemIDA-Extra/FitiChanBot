using FitiChanBot.Interfaces;
using FitiChanBot.Settings;

namespace FitiChanBot
{
    public class BackgroundMonitorService
    {
        public bool IsActive { get; private set; }
        private readonly MessageManagerService _msgManager;
        private readonly DelaySettings _delaySetting;
        private readonly CancellationTokenSource delaysCancel;
        public BackgroundMonitorService(MessageManagerService msgManager, FitiSettings fitiSettings)
        {
            _msgManager = msgManager;
            _delaySetting = fitiSettings.DelaySettings;
            IsActive = false;
            delaysCancel = new CancellationTokenSource();
        }

        private async Task BigMonitoringLoop(CancellationToken delayCancel)
        {
            while (IsActive == true)
            {
                _msgManager.PrepareShortList(_delaySetting.BigDelay);

                if (!_msgManager.IsShortMessagesListEmpty)
                    await SmallMonitoringLoop(delayCancel);

                try { await Task.Delay(_delaySetting.BigDelay, delayCancel); }
                catch (TaskCanceledException) { break; }
            }
        }

        private async Task SmallMonitoringLoop(CancellationToken delayCancel)
        {
            while (!_msgManager.IsShortMessagesListEmpty)
            {
                await _msgManager.SendShortListAsync();
                try { await Task.Delay(_delaySetting.SmallDelay, delayCancel); }
                catch (TaskCanceledException) { break; }
            }
        }

        public async Task StartAsync()
        {
            if (IsActive == true) throw new Exception("The monitoring service is already running. Stop it before starting it again");
            IsActive = true;
            await BigMonitoringLoop(delaysCancel.Token);
        }
        public void Stop()
        {
            IsActive = false;
            delaysCancel.Cancel();
        }
    }
}
