using System.Collections.ObjectModel;
using DataModule.Model;

namespace DataModule
{
    public class AnimalService : IAnimalService
    {
        public ObservableCollection<string> AnimalList { get; }
        private Dictionary<string, Animal> AnimalDictionary { get; } = new();
        public AnimalService(IAnimalData animalData)
        {
            this.AnimalList = new ObservableCollection<string>(animalData.Animals.Select(x => x.Name).ToList());
            foreach (var animal in animalData.Animals)
            {
                AnimalDictionary.Add(animal.Name, animal);
            }
        }

        public Animal GetAnimalDetails(string name)
        {
            if (AnimalDictionary.TryGetValue(name, out var details))
            {
                return details;
            }
            throw new Exception("데이터에 존재하지 않는 동물입니다.");
        }
    }
}
