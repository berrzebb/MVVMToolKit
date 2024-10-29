using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using Microsoft.Win32;
using MVVMToolKit.Interfaces;
namespace MVVMToolKit.Templates
{
    /// <summary>
    /// 팝업을 위한 기본 설정을 해둔 윈도우 객체
    /// </summary>
    public abstract class PopupWindow : Window
    {
        #region DWM WINAPI
        private enum DWMWINDOWATTRIBUTE
        {
            /// <summary>
            /// Flags used by the DwmSetWindowAttribute function to specify the rounded corner preference for a window.
            /// </summary>
            DwmwaWindowCornerPreference = 33
        }

        private enum DWM_WINDOW_CORNER_PREFERENCE
        {
            /// <summary>
            /// 
            /// </summary>
            Default = 0,
            /// <summary>
            /// 
            /// </summary>
            DoNotRound = 1,
            /// <summary>
            /// 
            /// </summary>
            Round = 2,
            /// <summary>
            /// 
            /// </summary>
            RoundSmall = 3
        }

        [DllImport("dwmapi.dll", CharSet = CharSet.Unicode, PreserveSig = false)]
        private static extern void DwmSetWindowAttribute(IntPtr handle,
            DWMWINDOWATTRIBUTE attribute,
            ref DWM_WINDOW_CORNER_PREFERENCE pvAttribute,
            uint cbAttribute);

        private static Version GetOsVersionFromRegistry()
        {
            int major = 0;
            // The 'CurrentMajorVersionNumber' string value in the CurrentVersion key is new for Windows 10,
            // and will most likely (hopefully) be there for some time before MS decides to change this - again...
            if (
                    TryGetRegistryKey(
                        @"SOFTWARE\Microsoft\Windows NT\CurrentVersion",
                        "CurrentMajorVersionNumber",
                        out object? majorObj
                    )
                )
            {
                majorObj ??= 0;

                major = (int)majorObj;
            }
            // When the 'CurrentMajorVersionNumber' value is not present we fallback to reading the previous key used for this: 'CurrentVersion'
            else if (
                TryGetRegistryKey(
                    @"SOFTWARE\Microsoft\Windows NT\CurrentVersion",
                    "CurrentVersion",
                    out object? version
                )
            )
            {
                version ??= string.Empty;

                string[] versionParts = ((string)version).Split('.');

                if (versionParts.Length >= 2)
                {
                    major = int.TryParse(versionParts[0], out int majorAsInt) ? majorAsInt : 0;
                }
            }

            int minor = 0;
            // The 'CurrentMinorVersionNumber' string value in the CurrentVersion key is new for Windows 10,
            // and will most likely (hopefully) be there for some time before MS decides to change this - again...
            if (
                    TryGetRegistryKey(
                        @"SOFTWARE\Microsoft\Windows NT\CurrentVersion",
                        "CurrentMinorVersionNumber",
                        out object? minorObj
                    )
                )
            {
                minorObj ??= string.Empty;

                minor = (int)minorObj;
            }
            // When the 'CurrentMinorVersionNumber' value is not present we fallback to reading the previous key used for this: 'CurrentVersion'
            else if (
                TryGetRegistryKey(
                    @"SOFTWARE\Microsoft\Windows NT\CurrentVersion",
                    "CurrentVersion",
                    out object? version
                )
            )
            {
                version ??= string.Empty;

                string[] versionParts = ((string)version).Split('.');

                if (versionParts.Length >= 2)
                    minor = int.TryParse(versionParts[1], out int minorAsInt) ? minorAsInt : 0;
            }

            int build = 0;
            if (
                    TryGetRegistryKey(
                        @"SOFTWARE\Microsoft\Windows NT\CurrentVersion",
                        "CurrentBuildNumber",
                        out object? buildObj
                    )
                )
            {
                buildObj ??= string.Empty;

                build = int.TryParse((string)buildObj, out int buildAsInt) ? buildAsInt : 0;
            }

            return new Version(major, minor, build);
        }
        private static bool TryGetRegistryKey(string path, string key, out object? value)
        {
            value = null;

            try
            {
                using RegistryKey? rk = Registry.LocalMachine.OpenSubKey(path);

                if (rk == null)
                {
                    return false;
                }

                value = rk.GetValue(key);

                return value != null;
            }
            catch
            {
                return false;
            }
        }
        #endregion
        #region Dependency Properties
        /// <summary>
        /// 팝업 TitleBar Height
        /// </summary>
        public static readonly DependencyProperty CaptionHeightProperty = DependencyProperty.Register(nameof(CaptionHeight), typeof(double), typeof(PopupWindow), new PropertyMetadata(34.0));

        /// <summary>
        /// 팝업 TitleBar Caption 높이
        /// </summary>
        public double? CaptionHeight
        {
            get
            {
                return (double)GetValue(CaptionHeightProperty);

            }
            set { SetValue(CaptionHeightProperty, value); }
        }
        /// <summary>
        /// 팝업 TitleBar Template
        /// </summary>
        public static readonly DependencyProperty TitleBarTemplateProperty = DependencyProperty.Register(nameof(TitleBarTemplate), typeof(DataTemplate), typeof(PopupWindow), new PropertyMetadata(null));
        #endregion
        /// <summary>
        /// 팝업 TitleBar Template
        /// </summary>
        public DataTemplate? TitleBarTemplate
        {
            get
            {
                return (DataTemplate)GetValue(TitleBarTemplateProperty);

            }
            set
            {
                if (value != null)
                {
                    SetValue(TitleBarTemplateProperty, value);
                }
            }
        }

        /// <summary>
        /// DWM을 사용할지 말지 설정합니다.
        /// </summary>
        public static readonly DependencyProperty IsDWMProperty = DependencyProperty.Register(nameof(IsDWM), typeof(bool), typeof(PopupWindow), new PropertyMetadata(false));

        private bool _isDWMInitialized = false;

        /// <summary>
        /// DWM 옵션을 사용할지 말지 설정합니다.
        /// </summary>
        /// 값이 <value><c>true</c> 라면 DWM을 사용합니다. </value>
        public bool IsDWM
        {
            get
            {
                return (bool)GetValue(IsDWMProperty);

            }
            set
            {
                SetValue(IsDWMProperty, value);
            }
        }

        /// <summary>
        /// 팝업 윈도우 객체 생성자
        /// </summary>
        protected PopupWindow()
        {
            _ = CommandBindings.Add(new CommandBinding(SystemCommands.CloseWindowCommand, CloseWindow));
            _ = CommandBindings.Add(new CommandBinding(SystemCommands.MaximizeWindowCommand, MaximizeWindow, CanResizeWindow));
            _ = CommandBindings.Add(new CommandBinding(SystemCommands.MinimizeWindowCommand, MinimizeWindow, CanMinimizeWindow));
            _ = CommandBindings.Add(new CommandBinding(SystemCommands.RestoreWindowCommand, RestoreWindow, CanResizeWindow));
            _ = CommandBindings.Add(new CommandBinding(SystemCommands.ShowSystemMenuCommand, ShowSystemMenu));

            DataContextChanged += PopupWindow_DataContextChanged;
        }

        private void PopupWindow_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (IsDWM && !_isDWMInitialized)
            {
                Version osVersion = GetOsVersionFromRegistry();
                if (osVersion.Build >= 22000)
                {
                    WindowInteropHelper handle = new(this);
                    IntPtr hWnd = handle.EnsureHandle();
                    DWMWINDOWATTRIBUTE attribute = DWMWINDOWATTRIBUTE.DwmwaWindowCornerPreference;
                    DWM_WINDOW_CORNER_PREFERENCE preference = DWM_WINDOW_CORNER_PREFERENCE.Round;
                    DwmSetWindowAttribute(hWnd, attribute, ref preference, sizeof(uint));
                    _isDWMInitialized = true;
                }
            }
        }

        #region Window Commands  

        /// <summary>
        /// 창의 Resize 가능 여부.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnCanResizeWindow(CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ResizeMode is ResizeMode.CanResize or ResizeMode.CanResizeWithGrip;

        }
        /// <summary>
        /// 창의 최소화 가능 여부.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnCanMinimizeWindow(CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ResizeMode != ResizeMode.NoResize;

        }
        private void CanResizeWindow(object sender, CanExecuteRoutedEventArgs e)
        {
            OnCanResizeWindow(e);
        }

        private void CanMinimizeWindow(object sender, CanExecuteRoutedEventArgs e)
        {
            OnCanMinimizeWindow(e);
        }
        /// <summary>
        /// 창을 닫습니다.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnCloseWindow(ExecutedRoutedEventArgs e)
        {
            if (DataContext is IDialogContext context)
            {
                context.Cleanup();
            }

            Close();

        }
        /// <summary>
        /// 창을 최대화 합니다.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnMaximizeWindow(ExecutedRoutedEventArgs e)
        {
            SystemCommands.MaximizeWindow(this);
        }
        /// <summary>
        /// 창을 최소화 합니다.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnMinimizeWindow(ExecutedRoutedEventArgs e)
        {
            SystemCommands.MinimizeWindow(this);
        }
        /// <summary>
        /// 창을 되돌립니다.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnRestoreWindow(ExecutedRoutedEventArgs e)
        {
            SystemCommands.RestoreWindow(this);
        }
        /// <summary>
        /// 시스템 메시지 메뉴를 호출합니다.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnShowSystemMenu(ExecutedRoutedEventArgs e)
        {
            if (e.OriginalSource is not FrameworkElement element)
                return;

            Point point = WindowState == WindowState.Maximized ? new Point(0, element.ActualHeight)
                : new Point(Left + BorderThickness.Left, element.ActualHeight + Top + BorderThickness.Top);
            point = element.TransformToAncestor(this).Transform(point);
            SystemCommands.ShowSystemMenu(this, point);
        }
        private void CloseWindow(object sender, ExecutedRoutedEventArgs e)
        {
            OnCloseWindow(e);
        }

        private void MaximizeWindow(object sender, ExecutedRoutedEventArgs e)
        {
            OnMaximizeWindow(e);
        }

        private void MinimizeWindow(object sender, ExecutedRoutedEventArgs e)
        {
            OnMinimizeWindow(e);
        }

        private void RestoreWindow(object sender, ExecutedRoutedEventArgs e)
        {
            OnRestoreWindow(e);
        }

        private void ShowSystemMenu(object sender, ExecutedRoutedEventArgs e)
        {
            OnShowSystemMenu(e);
        }

        #endregion

    }
}
