using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace DataModule.Model
{
    public class Animal
    {
        public int PresenterType { get; }
        public string Details { get; }
        public BitmapImage? Img { get; }
        public string Name { get; }
        public Animal(int presenterType, string details, BitmapImage img, string name)
        {
            PresenterType = presenterType;
            Details = details;
            Img = img;
            Name = name;
        }

        public Animal(int presenterType, string details, string name)
        {
            PresenterType = presenterType;
            Details = details;
            Name = name;
        }
    }
}
