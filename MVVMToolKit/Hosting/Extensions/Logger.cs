using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;

namespace MVVMToolKit.Hosting.Extensions
{
    /// <summary>
    /// Logger 확장
    /// </summary>
    public static partial class LoggerExtensions
    {
        /// <summary>
        /// ILogger 인터페이스를 이용하여 Trace 로그를 남깁니다.
        /// </summary>
        /// <param name="logger">사용할 로거</param>
        /// <param name="tag">로거 태그</param>
        /// <param name="message"></param>
        [LoggerMessage(EventId = 0, Level = LogLevel.Trace, Message = "[{tag}] {message}", SkipEnabledCheck = true)]
        public static partial void Trace(this ILogger logger, string message, [CallerMemberName] string tag = "");
        /// <summary>
        /// ILogger 인터페이스를 이용하여 Debug 로그를 남깁니다.
        /// </summary>
        /// <param name="logger">사용할 로거</param>
        /// <param name="tag">로거 태그</param>
        /// <param name="message"></param>
        [LoggerMessage(EventId = 0, Level = LogLevel.Debug, Message = "[{tag}] {message}")]
        public static partial void Debug(this ILogger logger, string message, [CallerMemberName] string tag = "");
        /// <summary>
        /// ILogger 인터페이스를 이용하여 Information 로그를 남깁니다.
        /// </summary>
        /// <param name="logger">사용할 로거</param>
        /// <param name="tag">로거 태그</param>
        /// <param name="message"></param>
        [LoggerMessage(EventId = 0, Level = LogLevel.Information, Message = "[{tag}] {message}")]
        public static partial void Information(this ILogger logger, string message, [CallerMemberName] string tag = "");
        /// <summary>
        /// ILogger 인터페이스를 이용하여 Warning 로그를 남깁니다.
        /// </summary>
        /// <param name="logger">사용할 로거</param>
        /// <param name="tag">로거 태그</param>
        /// <param name="message"></param>
        [LoggerMessage(EventId = 0, Level = LogLevel.Warning, Message = "[{tag}] {message}")]
        public static partial void Warning(this ILogger logger, string message, [CallerMemberName] string tag = "");
        /// <summary>
        /// ILogger 인터페이스를 이용하여 Error 로그를 남깁니다.
        /// </summary>
        /// <param name="logger">사용할 로거</param>
        /// <param name="tag">로거 태그</param>
        /// <param name="message"></param>
        [LoggerMessage(EventId = 0, Level = LogLevel.Error, Message = "[{tag}] {message}")]
        public static partial void Error(this ILogger logger, string message, [CallerMemberName] string tag = "");

        /// <summary>
        /// ILogger 인터페이스를 이용하여 Critical 로그를 남깁니다.
        /// </summary>
        /// <param name="logger">사용할 로거</param>
        /// <param name="tag">로거 태그</param>
        /// <param name="message"></param>
        [LoggerMessage(EventId = 0, Level = LogLevel.Critical, Message = "[{tag}] {message}")]
        public static partial void Critical(this ILogger logger, string message, [CallerMemberName] string tag = "");

        /// <summary>
        /// ILogger 인터페이스를 이용하여 로그를 남깁니다.
        /// </summary>
        /// <param name="logger">사용할 로거</param>
        /// <param name="logLevel">로깅 레벨</param>
        /// <param name="tag">로거 태그</param>
        /// <param name="message"></param>
        [LoggerMessage(EventId = 0, Message = "[{tag}] {message}")]
        public static partial void Log(this ILogger logger, LogLevel logLevel, string message, [CallerMemberName] string tag = "");
    }
}
