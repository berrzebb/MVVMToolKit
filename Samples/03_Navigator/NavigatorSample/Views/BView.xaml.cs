using System.Windows.Controls;
using MVVMToolKit.Attributes;
namespace NavigatorSample.Views
{
    [Navigatable(RouteName = "BView")]
    public partial class BView : UserControl
    {
        public BView()
        {
            InitializeComponent();
        }
    }
}
