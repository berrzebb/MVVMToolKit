namespace MVVMToolKit.Helper
{
    public static class TimeHelper
    {
        private static readonly DateTime UnixOrigin = new(1970, 1, 1, 0, 0, 0, 0);
        private const int ConvertHour = 3600 * 9; //GMT에서 한국시간 -9시간

        public static DateTime UnixTime2DateTime(int timestamp)
        {
            var dTimestamp = Convert.ToDouble(timestamp);
            return TimeHelper.UnixTime2DateTime(dTimestamp);
        }

        public static DateTime UnixTime2DateTime(double timestamp) => UnixOrigin.AddSeconds(timestamp).ToLocalTime();

        public static int DateTime2UnixTime(DateTime date)
        {
            TimeSpan diff = date - UnixOrigin;
            return Convert.ToInt32(diff.TotalSeconds) - ConvertHour;
        }

        public static int GetCurrentUnixTime()
        {
            TimeSpan timeSpan = (DateTime.UtcNow - UnixOrigin);
            return Convert.ToInt32(timeSpan.TotalSeconds);
        }
    }
}