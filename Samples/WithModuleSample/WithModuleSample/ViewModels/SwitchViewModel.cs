using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MVVMToolKit.ViewModels;

namespace WithModuleSample
{
    using MVVMToolKit.Interfaces;

    public partial class SwitchViewModel : ViewModelBase<SwitchViewModel>, IViewSelector
    {
        [ObservableProperty] private int _selector;
        /// <inheritdoc />
        public SwitchViewModel(IServiceProvider provider) : base(provider)
        {
        }

        [RelayCommand]
        private void ChangeView(string viewType)
        {
            switch (viewType)
            {
                case "First":
                    Selector = 0;
                    break;
                case "Secondary":
                    Selector = 1;
                    break;

            }
        }

        /// <inheritdoc />
        public string NavigateTo() => Selector switch
        {
            0 => nameof(FirstView),
            1 => nameof(SecondaryView),
            _ => nameof(FirstView)
        };
    }
}
