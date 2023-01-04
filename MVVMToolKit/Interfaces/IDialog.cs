namespace MVVMToolKit.Interfaces
{
    /// <summary>
    /// The dialog interface
    /// </summary>
    public interface IDialog
    {
        /// <summary>
        /// Gets or sets the value of the title
        /// </summary>
        public string? Title { get; set; }
        /// <summary>
        /// Gets or sets the value of the width
        /// </summary>
        public double Width { get; set; }
        /// <summary>
        /// Gets or sets the value of the height
        /// </summary>
        public double Height { get; set; }
        /// <summary>
        /// Gets or sets the value of the data context
        /// </summary>
        object? DataContext { get; set; }

        /// <summary>
        /// Describes whether this instance activate
        /// </summary>
        /// <returns>The bool</returns>
        bool Activate();

        /// <summary>
        /// Shows this instance
        /// </summary>
        void Show();
        /// <summary>
        /// Shows the dialog
        /// </summary>
        /// <returns>The bool</returns>
        bool? ShowDialog();
        /// <summary>
        /// Closes this instance
        /// </summary>
        void Close();
        /// <summary>
        /// Gets or sets the value of the on close
        /// </summary>
        Action? OnClose { get; set; }
    }
}
