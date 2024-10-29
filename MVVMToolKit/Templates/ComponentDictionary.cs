using System.Windows.Markup;

namespace MVVMToolKit.Templates
{
    /// <inheritdoc />
    [Localizability(LocalizationCategory.Ignore)]
    [Ambient]
    [UsableDuringInitialization(true)]
    public class ComponentDictionary : ResourceDictionary
    {
        /// <summary>
        /// The library URL
        /// </summary>
        public const string LibraryUrl = $"pack://application:,,,/EnChart.Enc;component/Resources/generic.xaml";
        /// <summary>
        /// The presentation URL
        /// </summary>
        public const string PresentationUrl = $"pack://application:,,,/MVVMToolKit;component/Resources/generic.xaml";

        /// <inheritdoc />
        public ComponentDictionary()
        {
#if MERGED
            Source = new Uri(LibraryUrl, UriKind.Absolute);
#else
            Source = new Uri(PresentationUrl, UriKind.Absolute);
#endif
        }
    }
}
