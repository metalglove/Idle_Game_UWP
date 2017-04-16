using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Windows.UI.Xaml.Media.Imaging;

namespace Idle_Game_UWP.Classes
{
    class User
    {
        public float Gold = 100000.00f;
        public float GoldPerClick = 1.00f;
        public float GoldPerSecond = 0.00f;

        public List<Item> Items = new List<Item>();
        public List<Upgrade> Upgrades = new List<Upgrade>();

        private XMLReader Reader = new XMLReader();

        public User()
        {
            LoadItems();
            Task.Run((Action)LoadUpgrades);
            /*
             * the upgrade list had to be filled on a different thread/task else it wouldn't load or it would display the wrong list of items/upgrades
             */
        }
        public void Clicked()
        {
            Gold += GoldPerClick;
        }
        private void LoadItems()
        {
            XmlDocument XmlDocument = Reader.ReadXMLAsync(new Uri("ms-appx:///Content/Items.xml")).Result;
            lock (this)
            {
                foreach (XmlNode node in XmlDocument.DocumentElement.ChildNodes)
                {
                    int[] innerItem = new int[3];
                    int i = 0;
                    foreach (XmlNode item in node.ChildNodes)
                    {
                        innerItem[i] = int.Parse(item.InnerText);
                        i++;
                    }
                    BitmapImage tempImage = new BitmapImage(new Uri(node.Attributes["image"].Value));
                    Items.Add(new Item(node.Attributes["name"].Value, tempImage, innerItem[0], innerItem[1], innerItem[2]));
                }
            }
        }
        private void LoadUpgrades()
        {
            XmlDocument XmlDocument = Reader.ReadXMLAsync(new Uri("ms-appx:///Content/Upgrades.xml")).Result;
            lock (this)
            {
                foreach (XmlNode node in XmlDocument.DocumentElement.ChildNodes)
                {
                    int[] innerItem = new int[3];
                    int i = 0;
                    foreach (XmlNode item in node.ChildNodes)
                    {
                        innerItem[i] = int.Parse(item.InnerText);
                        i++;
                    }
                    Upgrades.Add(new Upgrade(node.Attributes["name"].Value, innerItem[0], innerItem[1], innerItem[2]));
                }
            }
        }
    }
}
