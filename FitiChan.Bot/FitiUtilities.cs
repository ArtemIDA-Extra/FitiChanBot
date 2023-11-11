using Discord;

namespace FitiChanBot
{
    public static class FitiUtilities
    {
        public static TimeSpan UTCOffsetCalculate(DateTime local)
        {
            if (DateTime.Compare(DateTime.UtcNow, local) > 0)
            {
                TimeSpan utcOffset = DateTime.UtcNow - local;
                //Text = $"{DateTime.UtcNow.ToString("H:mm")} (UTC) / {(DateTime.UtcNow - utcOffset).ToString("H:mm")} (Local, UTC -{utcOffset.ToString("hh\\:mm")})"
                return new TimeSpan(0, 0, 0, 0, 0) - utcOffset;
            }
            else if (DateTime.Compare(DateTime.UtcNow, local) < 0)
            {
                TimeSpan utcOffset = local - DateTime.UtcNow;
                //Text = $"{DateTime.UtcNow.ToString("H:mm")} (UTC) / {(DateTime.UtcNow + utcOffset).ToString("H:mm")} (Local, UTC +{utcOffset.ToString("hh\\:mm")})"
                return new TimeSpan(0, 0, 0, 0, 0) + utcOffset;
            }
            else
            {
                return new TimeSpan(0,0,0,0,0);
            }
        }
    }
}
