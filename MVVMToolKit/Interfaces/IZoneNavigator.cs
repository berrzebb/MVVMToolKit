namespace MVVMToolKit.Interfaces
{
    using System.Threading.Tasks;

    public interface IZoneNavigator
    {
        Task Navigate(string zoneName, string routeName);
    }
}
