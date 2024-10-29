using System.Collections.ObjectModel;
using DataModule.Model;

namespace DataModule
{
    public interface IAnimalService
    {
        ObservableCollection<string> AnimalList { get; }
        Animal GetAnimalDetails(string name);
    }
}
