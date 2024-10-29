using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DataModule;
using MVVMToolKit;
using MVVMToolKit.Attributes;
using MVVMToolKit.Interfaces;
using PresenterModule.Views;

namespace PresenterModule.ViewModels
{
    [Navigatable(RouteName = "Animal", ViewName = nameof(AnimalInformationView))]
    public partial class AnimalInfomationViewModel : ViewModelBase<AnimalInfomationViewModel>, IViewSelector
    {
        private readonly IAnimalService _animalService;

        public ReadOnlyObservableCollection<string> Animals { get; }

        [ObservableProperty]
        private string _details = "";
        [ObservableProperty]
        private BitmapImage? _animalImage;

        public AnimalInfomationViewModel(IAnimalService animalService)
        {
            _animalService = animalService;
            Animals = new ReadOnlyObservableCollection<string>(animalService.AnimalList);
        }

        [ObservableProperty]
        private string _selectedAnimal = string.Empty;

        [RelayCommand]
        private void ShowDetail()
        {
            if (string.IsNullOrWhiteSpace(SelectedAnimal)) return;

            var animal = _animalService.GetAnimalDetails(SelectedAnimal);

            switch (animal.PresenterType)
            {
                case 1:
                    //텍스트
                    this.Details = animal.Details;
                    ChangeView("Text");
                    break;
                case 2:
                    //텍스트 + 이미지
                    this.Details = animal.Details;
                    this.AnimalImage = animal.Img;
                    ChangeView("TextWithImage");
                    break;
                default:
                    return;
            }
        }

        [RelayCommand]
        private void ChangeView(string dataType)
        {
            switch (dataType)
            {
                case "List":
                    NavigateTo(nameof(AnimalInformationView));
                    break;
                case "Text":
                    NavigateTo(nameof(TextView));
                    break;
                case "TextWithImage":
                    NavigateTo(nameof(TextWithImageView));
                    break;
            }
        }
    }
}
