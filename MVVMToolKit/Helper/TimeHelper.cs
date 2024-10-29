using System;

namespace MVVMToolKit.Helper
{
    /// <summary>
    /// 시간 관련 기능을 도와줄 수 있는 Helper 객체
    /// </summary>
    public static class TimeHelper
    {
        private const int ConvertHour = 3600 * 9; //GMT에서 한국시간 -9시간
        /// <summary>
        /// Unix Time을 DateTime으로 변환합니다.
        /// </summary>
        /// <param name="timestamp">Unix Timestamp</param>
        /// <returns>DataTime</returns>
        public static DateTime UnixTime2DateTime(int timestamp)
        {
            var dTimestamp = Convert.ToDouble(timestamp);
            return UnixTime2DateTime(dTimestamp);
        }
        /// <summary>
        /// Unix Time을 DateTime으로 변환합니다.
        /// </summary>
        /// <param name="timestamp">Unix Time Stamp</param>
        /// <returns>DateTime(Local Time)</returns>
        public static DateTime UnixTime2DateTime(double timestamp) => DateTime.UnixEpoch.AddSeconds(timestamp).ToLocalTime();
        /// <summary>
        /// DateTime을 Unix TimeStamp로 변환합니다.
        /// </summary>
        /// <param name="date">DateTime</param>
        /// <returns>Unix TimeStamp</returns>
        public static int DateTime2UnixTime(DateTime date)
        {
            TimeSpan diff = date - DateTime.UnixEpoch;
            return Convert.ToInt32(diff.TotalSeconds) - ConvertHour;
        }
        /// <summary>
        /// 현재 UnixTimeStamp를 획득합니다.
        /// </summary>
        /// <returns>현재 Unix TimeStamp.</returns>
        public static int GetCurrentUnixTime()
        {
            TimeSpan timeSpan = DateTime.UtcNow - DateTime.UnixEpoch;
            return Convert.ToInt32(timeSpan.TotalSeconds);
        }
    }
}
