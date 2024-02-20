using System.Threading;
using MVVMToolKit.Helper.Native;

namespace MVVMToolKit.Helper
{
    /// <summary>
    /// The process helper class.
    /// </summary>
    public class ProcessHelper
    {
            /// <summary>
            /// The mutex.
            /// </summary>
            private static Mutex? _mutex = null;

    /// <summary>
    /// Application의 Instance 실행 여부.
    /// </summary>
    /// <param name="processName">프로세스 명칭.</param>
    /// <returns>
    /// 실행중 = true / 실행중인 프로세스 없음 = false.
    /// </returns>
    public static bool IsRunningProcess(string processName)
    {
        try
        {
            _mutex = new Mutex(false, processName);
        }
        catch (Exception)
        {
            return true;
        }

        try
        {
            if (!_mutex.WaitOne(0, false))
            {
                return true;
            }
        }
        catch
        {
            return true;
        }

        return false;
    }
    
    /// <summary>
    /// 현재 활성화된 WindowHandle을 찾는다.
    /// </summary>
    /// <param name="className">Class Name.</param>
    /// <param name="windowName">Window Title.</param>
    /// <returns>IntPtr.</returns>
    public static IntPtr FindWindow(string className, string windowName)
    {
        return NativeMethods.FindWindow(className, windowName);
    }

    /// <summary>
    /// 해당 핸들을 가진 Window를 Foreground로 Activate 시킨다.
    /// </summary>
    /// <param name="hWnd">Handle.</param>
    /// <returns>Int Ptr.</returns>
    public static IntPtr SetForegroundWindow(IntPtr hWnd)
    {
        return NativeMethods.SetForegroundWindow(hWnd);
    }

    /// <summary>
    /// 해당 핸들의 Window가 minimize된 상태인지 확인한다.
    /// </summary>
    /// <param name="hwnd">Handle.</param>
    /// <returns>bool.</returns>
    public static bool IsIconic(IntPtr hwnd)
    {
        return NativeMethods.IsIconic(hwnd);
    }

    /// <summary>
    /// 해당 핸들의 Window를 Command 상태로 전환한다.
    /// </summary>
    /// <param name="hwnd">Handle.</param>
    /// <param name="command">Command.</param>
    /// <returns>int.</returns>
    public static int ShowWindow(IntPtr hwnd, NativeMethods.WindowShowStyle command)
    {
        return NativeMethods.ShowWindow(hwnd, (int)command);
    }
    }
}
