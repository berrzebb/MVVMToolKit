using MVVMToolKit.Interfaces;

namespace MVVMToolKit.Models
{
    public class PopupOption
    {
        public string? Title { get; set; } = "";
        public double Width { get; set; } = 0;
        public double Height { get; set; } = 0;
        public string HostType { get; set; } = DialogHostType.Default.ToString();
        public bool Topmost { get; set; } = true;
        public bool IsModal { get; set; } = true;
        public bool ShowInTaskbar { get; set; } = false;
        public bool ShowActivated { get; set; } = false;
        public SizeToContent SizeToContent { get; set; } = SizeToContent.Manual;
        public WindowStyle WindowStyle { get; set; } = WindowStyle.ToolWindow;
        public WindowStartupLocation WindowStartupLocation { get; set; } = WindowStartupLocation.CenterScreen;
        public ResizeMode ResizeMode { get; set; } = ResizeMode.NoResize;

    }
}
