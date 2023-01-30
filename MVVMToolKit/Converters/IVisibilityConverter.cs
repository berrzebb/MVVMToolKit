using System.Windows.Data;

namespace MVVMToolKit.Converters
{
    public interface IVisibilityConverter : IValueConverter
    {
        Visibility TrueValue {get; set;}
        Visibility FalseValue {get; set;}
    }
}
