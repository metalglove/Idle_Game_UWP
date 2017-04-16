using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace Idle_Game_UWP.Classes
{
    public class Item
    {
        public string Name { get; private set; }
        public float Cost { get; set; }
        public int Power { get; set; }
        public int Count { get; set; }
        public BitmapImage Image {  get; private set; }
        public Item(string Name, BitmapImage Image, int Cost, int Count, int Power)
        {
            this.Name = Name;
            this.Image = Image;
            this.Cost = Cost;
            this.Power = Power;
            this.Count = Count;
        }
    }
}
