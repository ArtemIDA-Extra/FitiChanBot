namespace FitiChanBot
{
    public class BackgroundMonitorService
    {
        public bool IsActive { get; private set; }
        public TimeSpan BigDelay { get; private set; }
        public TimeSpan SmallDelay { get; private set; }

        private readonly MessageManagerService _msgManager;
        private readonly CancellationTokenSource delaysCancel;
        public BackgroundMonitorService(MessageManagerService msgManager, TimeSpan bigDelay, TimeSpan smallDelay)
        {
            _msgManager = msgManager;
            BigDelay = bigDelay;
            SmallDelay = smallDelay;
            IsActive = false;
            delaysCancel = new CancellationTokenSource();
        }

        private async Task BigMonitoringLoop(CancellationToken delayCancel)
        {
            while (IsActive == true)
            {
                _msgManager.PrepareShortList(BigDelay);

                if (!_msgManager.IsShortMessagesListEmpty)
                    await SmallMonitoringLoop(delayCancel);

                try { await Task.Delay(BigDelay, delayCancel); }
                catch (TaskCanceledException) { break; }
            }
        }

        private async Task SmallMonitoringLoop(CancellationToken delayCancel)
        {
            while(!_msgManager.IsShortMessagesListEmpty)
            {
                await _msgManager.SendShortListAsync();
                try { await Task.Delay(SmallDelay, delayCancel); }
                catch (TaskCanceledException) { break; }
            }
        }

        public async Task StartAsync()
        {
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
