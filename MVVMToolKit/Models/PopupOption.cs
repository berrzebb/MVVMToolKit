namespace MVVMToolKit.Models
{
    public class PopupOption
    {
        public string? Title { get; set; } = "";

        public double Width { get; set; } = 0;
        public double Height { get; set; } = 0;

        public bool IsDependencyInjection { get; set; } = true;

        public string HostType { get; set; } = "Default";

        public DataTemplate? TitleBarTemplate { get; set; } = null;

    }
}
