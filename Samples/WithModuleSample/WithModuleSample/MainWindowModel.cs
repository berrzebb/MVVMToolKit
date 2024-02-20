using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MVVMToolKit.Navigation.Mapping;
using MVVMToolKit.ViewModels;

namespace WithModuleSample
{
    public partial class MainWindowModel : ViewModelBase<MainWindowModel>, IViewSelector
    {
        [ObservableProperty] private int _selector;
        /// <inheritdoc />
        public MainWindowModel(IServiceProvider provider) : base(provider)
        {
        }

        [RelayCommand]
        private void ChangeView(string ViewType)
        {
            switch (ViewType)
            {
                case "First":
                    Selector = 0;
                    break;
                case "Secondary":
                    Selector = 1;
                    break;

            }
        }

        partial void OnSelectorChanged(int value)
        {

        }

        /// <inheritdoc />
        public string GetView()
        {
            return Selector switch
            {
                0 => nameof(FirstView),
                1 => nameof(SecondaryView),
                _ => nameof(FirstView)
            };
        }
    }
}
