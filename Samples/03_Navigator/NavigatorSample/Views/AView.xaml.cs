using System.Windows.Controls;
using MVVMToolKit.Attributes;
namespace NavigatorSample.Views
{
    [Navigatable(RouteName = "AView")]
    public partial class AView : UserControl
    {
        public AView()
        {
            InitializeComponent();
        }
    }
}
