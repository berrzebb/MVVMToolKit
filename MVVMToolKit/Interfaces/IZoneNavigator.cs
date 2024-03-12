namespace MVVMToolKit.Interfaces
{
    public class NavigationResult
    {
        public bool IsSuccess { get; private set; }
        public string? Reason { get; private set; }

        public NavigationResult(bool isSuccess = true, string reason = "")
        {
            IsSuccess = isSuccess;
            Reason = reason;
        }
    }
    public interface IZoneNavigator
    {
        NavigationResult Navigate(string zoneName, object routeKey, Type? contextType = null);
    }
}
