using MVVMToolKit.Enums.Dialog;

namespace MVVMToolKit.Models
{
    public class PopupOption
    {
        public string? Title { get; init; } = "";
        public double Width { get; init; } = 0;
        public double Height { get; init; } = 0;
        public string HostType { get; init; } = DialogHostType.Default.ToString();
        public bool Topmost { get; init; } = true;
        public bool IsModal { get; init; } = true;
        public bool ShowInTaskbar { get; init; } = false;
        public bool ShowActivated { get; init; } = false;
        public SizeToContent SizeToContent { get; init; } = SizeToContent.Manual;
        public WindowStyle WindowStyle { get; init; } = WindowStyle.ToolWindow;
        public WindowStartupLocation WindowStartupLocation { get; init; } = WindowStartupLocation.CenterScreen;
        public ResizeMode ResizeMode { get; init; } = ResizeMode.NoResize;

    }
}
