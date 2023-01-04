namespace MVVMToolKit.Helper
{
    public static class TimeHelper
    {
        private static readonly DateTime _unixOrigin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
        private static readonly int _convertHour = 3600 * 9; //GMT에서 한국시간 -9시간

        public static DateTime UnixTime2DateTime(int timestamp)
        {
            var dTimestamp = Convert.ToDouble(timestamp);
            return TimeHelper.UnixTime2DateTime(dTimestamp);
        }

        public static DateTime UnixTime2DateTime(double timestamp)
        {
            return _unixOrigin.AddSeconds(timestamp).ToLocalTime();
        }

        public static int DateTime2UnixTime(DateTime date)
        {
            TimeSpan diff = date - _unixOrigin;
            return Convert.ToInt32(diff.TotalSeconds) - _convertHour;
        }

        public static int GetCurrentUnixTime()
        {
            TimeSpan timeSpan = (DateTime.UtcNow - _unixOrigin);
            return Convert.ToInt32(timeSpan.TotalSeconds);
        }
    }
}