using System;

namespace MVVMToolKit.Interfaces
{
    public interface IDialog
    {
        public string? Title { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        object? DataContext { get; set; }

        bool Activate();

        void Show();
        bool? ShowDialog();
        void Close();
        Action? OnClose { get; set; }
    }
}
