using System.Runtime.InteropServices;
using System.Windows.Input;
using System.Windows.Interop;
using MVVMToolKit.Interfaces;

namespace MVVMToolKit.Templates
{
    /// <summary>
    /// PopupWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class PopupWindow : Window
    {
        #region DWM WINAPI
        public enum DWMWINDOWATTRIBUTE
        {
            DWMWA_WINDOW_CORNER_PREFERENCE = 33
        }

        public enum DWM_WINDOW_CORNER_PREFERENCE
        {
            DWMWCP_DEFAULT = 0,
            DWMWCP_DONOTROUND = 1,
            DWMWCP_ROUND = 2,
            DWMWCP_ROUNDSMALL = 3
        }

        [DllImport("dwmapi.dll", CharSet = CharSet.Unicode, PreserveSig = false)]
        internal static extern void DwmSetWindowAttribute(IntPtr handle,
            DWMWINDOWATTRIBUTE attribute,
            ref DWM_WINDOW_CORNER_PREFERENCE pvAttribute,
            uint cbAttribute);

        private static Version GetOSVersionFromRegistry()
        {
            int major = 0;
            {
                // The 'CurrentMajorVersionNumber' string value in the CurrentVersion key is new for Windows 10,
                // and will most likely (hopefully) be there for some time before MS decides to change this - again...
                if (
                    TryGetRegistryKey(
                        @"SOFTWARE\Microsoft\Windows NT\CurrentVersion",
                        "CurrentMajorVersionNumber",
                        out var majorObj
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
                        out var version
                    )
                )
                {
                    version ??= String.Empty;

                    var versionParts = ((string)version).Split('.');

                    if (versionParts.Length >= 2)
                    {
                        major = int.TryParse(versionParts[0], out int majorAsInt) ? majorAsInt : 0;
                    }
                }
            }

            int minor = 0;
            {
                // The 'CurrentMinorVersionNumber' string value in the CurrentVersion key is new for Windows 10,
                // and will most likely (hopefully) be there for some time before MS decides to change this - again...
                if (
                    TryGetRegistryKey(
                        @"SOFTWARE\Microsoft\Windows NT\CurrentVersion",
                        "CurrentMinorVersionNumber",
                        out var minorObj
                    )
                )
                {
                    minorObj ??= String.Empty;

                    minor = (int)minorObj;
                }
                // When the 'CurrentMinorVersionNumber' value is not present we fallback to reading the previous key used for this: 'CurrentVersion'
                else if (
                    TryGetRegistryKey(
                        @"SOFTWARE\Microsoft\Windows NT\CurrentVersion",
                        "CurrentVersion",
                        out var version
                    )
                )
                {
                    version ??= String.Empty;

                    var versionParts = ((string)version).Split('.');

                    if (versionParts.Length >= 2)
                        minor = int.TryParse(versionParts[1], out int minorAsInt) ? minorAsInt : 0;
                }
            }

            int build = 0;
            {
                if (
                    TryGetRegistryKey(
                        @"SOFTWARE\Microsoft\Windows NT\CurrentVersion",
                        "CurrentBuildNumber",
                        out var buildObj
                    )
                )
                {
                    buildObj ??= String.Empty;

                    build = int.TryParse((string)buildObj, out int buildAsInt) ? buildAsInt : 0;
                }
            }

            return new(major, minor, build);
        }
        private static bool TryGetRegistryKey(string path, string key, out object? value)
        {
            value = null;

            try
            {
                using var rk = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(path);

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
        public static readonly DependencyProperty CaptionHeightProperty = DependencyProperty.Register(nameof(CaptionHeight), typeof(double), typeof(PopupWindow), new PropertyMetadata(34.0, OnCaptionHeightChanged));

        public double? CaptionHeight
        {
            get
            {
                return (double)GetValue(CaptionHeightProperty);

            }
            set { SetValue(CaptionHeightProperty, value); }
        }

        private static void OnCaptionHeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }

        public static readonly DependencyProperty TitleBarTemplateProperty = DependencyProperty.Register(nameof(TitleBarTemplate), typeof(DataTemplate), typeof(PopupWindow), new PropertyMetadata(null, OnTitleBarTemplateChanged));
        #endregion
        public DataTemplate? TitleBarTemplate
        {
            get
            {
                return (DataTemplate)GetValue(TitleBarTemplateProperty);

            }
            set
            {
                if (value == null)
                {
                    SetValue(TitleBarTemplateProperty, Resources["DefaultTitleBarTemplate"] as DataTemplate);
                }
                else
                {
                    SetValue(TitleBarTemplateProperty, value);

                }
            }
        }

        private static void OnTitleBarTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {


        }

        public PopupWindow()
        {
            DefaultStyleKey = typeof(PopupWindow);

            CommandBindings.Add(new CommandBinding(SystemCommands.CloseWindowCommand, CloseWindow));
            CommandBindings.Add(new CommandBinding(SystemCommands.MaximizeWindowCommand, MaximizeWindow, CanResizeWindow));
            CommandBindings.Add(new CommandBinding(SystemCommands.MinimizeWindowCommand, MinimizeWindow, CanMinimizeWindow));
            CommandBindings.Add(new CommandBinding(SystemCommands.RestoreWindowCommand, RestoreWindow, CanResizeWindow));
            CommandBindings.Add(new CommandBinding(SystemCommands.ShowSystemMenuCommand, ShowSystemMenu));
            InitializeComponent();
            var osVersion = GetOSVersionFromRegistry();
            if (osVersion.Build >= 22000)
            {
                IntPtr hWnd = new WindowInteropHelper(this).EnsureHandle();
                var attribute = DWMWINDOWATTRIBUTE.DWMWA_WINDOW_CORNER_PREFERENCE;
                var preference = DWM_WINDOW_CORNER_PREFERENCE.DWMWCP_ROUND;
                DwmSetWindowAttribute(hWnd, attribute, ref preference, sizeof(uint));
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }
        #region Window Commands  

        private void CanResizeWindow(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ResizeMode == ResizeMode.CanResize || ResizeMode == ResizeMode.CanResizeWithGrip;
        }

        private void CanMinimizeWindow(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ResizeMode != ResizeMode.NoResize;
        }

        private void CloseWindow(object sender, ExecutedRoutedEventArgs e)
        {
            if (DataContext is IDialogContext context)
            {
                context.Cleanup();
            }
            this.Close();
        }

        private void MaximizeWindow(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.MaximizeWindow(this);
        }

        private void MinimizeWindow(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.MinimizeWindow(this);
        }

        private void RestoreWindow(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.RestoreWindow(this);
        }


        private void ShowSystemMenu(object sender, ExecutedRoutedEventArgs e)
        {
            var element = e.OriginalSource as FrameworkElement;
            if (element == null)
                return;

            var point = WindowState == WindowState.Maximized ? new Point(0, element.ActualHeight)
                : new Point(Left + BorderThickness.Left, element.ActualHeight + Top + BorderThickness.Top);
            point = element.TransformToAncestor(this).Transform(point);
            SystemCommands.ShowSystemMenu(this, point);
        }

        #endregion
    }
}
