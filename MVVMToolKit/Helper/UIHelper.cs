using System.Threading.Tasks;

namespace MVVMToolKit.Helper
{
    public static class UIThreadHelper
    {
        public static void CheckAndInvokeOnUIDispatcher(Action? act)
        {
            if (act is null)
            {
                return;
            }

            if (Application.Current.Dispatcher.CheckAccess())
            {
                act();
            }
            else
            {
                Application.Current.Dispatcher.BeginInvoke(act);
            }
        }
        public static Task CheckAndInvokeOnUIDispatcherUsingTask(Action? act)
        {
            if (act is null)
            {
                return Task.Delay(0);
            }

            if (Application.Current.Dispatcher.CheckAccess())
            {
                act();
                return Task.CompletedTask;
            }
            else
            {
                return Application.Current.Dispatcher.BeginInvoke(act).Task;
            }
        }
    }
}