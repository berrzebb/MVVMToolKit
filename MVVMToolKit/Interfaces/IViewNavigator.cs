namespace MVVMToolKit.Interfaces
{
    public class NavigationResult
    {
        public bool IsSuccess { get; private set; }
        public string? Reason { get; private set; }

        public NavigationResult(bool isSuccess = true, string reason = "")
        {
            this.IsSuccess = isSuccess;
            this.Reason = reason;
        }
    }
    public interface IViewNavigator
    {
        NavigationResult Navigate(string zoneName, object routeKey, Type? contextType = null);
    }
}
