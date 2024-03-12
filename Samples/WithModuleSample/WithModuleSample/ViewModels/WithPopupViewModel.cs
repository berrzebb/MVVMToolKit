using System.Windows;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using MVVMToolKit.Interfaces;
using MVVMToolKit.Models;
using MVVMToolKit.ViewModels;

namespace WithModuleSample.ViewModels
{
    public partial class WithPopupViewModel : ViewModelBase<WithPopupViewModel>
    {
        IDialogService _dialogService;
        List<IPopupContext> popupContexts = new();
        public WithPopupViewModel(IServiceProvider provider) : base(provider)
        {
            _dialogService = provider.GetRequiredService<IDialogService>();
        }
        [RelayCommand]
        private void ShowPopup()
        {
            var popupContext = _dialogService.CreateDialog(this, new PopupOption()
            {
                Title = "Test",
                Width = 400,
                Height = 800,
                TitleBarTemplate = Application.Current.Resources["CustomTitleBarTemplate"] as DataTemplate,
                IsDependencyInjection = false,
            });


            popupContexts.Add(popupContext
                .CloseListener(ClosePopup)
                .TitleBar(false)
                .ManualLocation()
                .Show());

        }
        private void ClosePopup(IPopupContext popupContext)
        {
            popupContexts.Remove(popupContext);
        }
        private void ApplyPopup(Action<IPopupContext> doAction)
        {
            foreach (var popup in popupContexts)
            {
                doAction(popup);
            }
        }
        [RelayCommand]
        private void ClosePopup()
        {
            var target = popupContexts.ToArray();
            foreach (var popup in target)
            {
                popup.Close();
            }
        }
        [RelayCommand]
        private void UpdateTitleWithMinimized()
        {

            ApplyPopup(popup => popup
                .Title("최소화 최대화가 포함된 팝업")
                .TitleBar(true, captionHeight: 40).Taskbar(true));

        }



        [RelayCommand]
        private void UpdateTitle()
        {
            ApplyPopup(popup => popup
                .Title("최소화 최대화가 없는 팝업")
                .TitleBar(true, true, captionHeight: 80));

        }
        [RelayCommand]
        private void DoTest()
        {
            MessageBox.Show("테스트 팝업");
        }

        [RelayCommand]
        private void DoActivate()
        {
            popupContexts[^1].Activate();
        }
    }
}
