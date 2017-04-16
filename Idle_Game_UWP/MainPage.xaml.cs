using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System.Threading;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Idle_Game_UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private Classes.User User = new Classes.User();
        private TimeSpan UpdatePeriod = TimeSpan.FromMilliseconds(50);

        public MainPage()
        {
            this.InitializeComponent();
            LoadContent();
            ThreadPoolTimer PeriodicTimer = ThreadPoolTimer.CreatePeriodicTimer(async (source) =>
            {
                await Dispatcher.RunAsync(CoreDispatcherPriority.High, Update);
            }, UpdatePeriod); 
        }
        private void Update()
        {
            TBGoldDisplay.Text = "Gold: " + User.Gold.ToString("F0");
            TBGoldPerClickDisplay.Text = "GPC: " + User.GoldPerClick.ToString();
            TBGoldPerSecondDisplay.Text = "GPS: " + User.GoldPerSecond.ToString();
            User.Gold += User.GoldPerSecond / 20;
            foreach (Button item in GButtonHolder.Children.OfType<Button>())
            {
                float tempFloat = (float)item.Tag;
                if (tempFloat <= User.Gold)
                {
                    item.Background = new SolidColorBrush(Colors.Green);
                    item.IsEnabled = true;
                }
                else
                {
                    item.Background = new SolidColorBrush(Colors.Red);
                    item.IsEnabled = false;
                }
            }
        }
        private void LoadContent()
        {
            LoadItems(null,null);
        }
        private void BTClick_Click(object sender, RoutedEventArgs e)
        {
            User.Clicked();
        }
        private void LoadUpgrades(object sender, RoutedEventArgs e)
        {
            GButtonHolder.Children.Clear();
            GButtonHolder.RowDefinitions.Clear();
            int GridRow = 0;
            foreach (Classes.Upgrade upgrade in User.Upgrades)
            {
                RowDefinition rDef = new RowDefinition();
                rDef.Height = new GridLength(100);
                GButtonHolder.RowDefinitions.Add(rDef);

                Image IMGItemImage = CreateImage(upgrade, GridRow, 0);
                TextBlock TBItemInfo = CreateTextBlock(upgrade, GridRow, 1);
                Button BTItemButton = CreateButton(upgrade, GridRow, 2);

                GButtonHolder.Children.Add(IMGItemImage);
                GButtonHolder.Children.Add(TBItemInfo);
                GButtonHolder.Children.Add(BTItemButton);

                GridRow++;
            }
        }
        private void LoadItems(object sender, RoutedEventArgs e)
        {
            GButtonHolder.Children.Clear();
            GButtonHolder.RowDefinitions.Clear();
            int GridRow = 0;
            foreach (Classes.Item item in User.Items)
            {
                RowDefinition rDef = new RowDefinition();
                rDef.Height = new GridLength(100);
                GButtonHolder.RowDefinitions.Add(rDef);

                Image IMGItemImage = CreateImage(item, GridRow, 0);
                TextBlock TBItemInfo = CreateTextBlock(item, GridRow, 1);
                Button BTItemButton = CreateButton(item, GridRow, 2);

                GButtonHolder.Children.Add(IMGItemImage);
                GButtonHolder.Children.Add(TBItemInfo);
                GButtonHolder.Children.Add(BTItemButton);

                GridRow++;
            }
        }
        private Image CreateImage(Classes.Item item, int GridRow, int GridColumn)
        {
            Image IMGItemImage = new Image();
            IMGItemImage.HorizontalAlignment = HorizontalAlignment.Stretch;
            IMGItemImage.VerticalAlignment = VerticalAlignment.Stretch;
            IMGItemImage.Source = item.Image;
            IMGItemImage.Stretch = Stretch.Fill;
            Grid.SetRow(IMGItemImage, GridRow);
            Grid.SetColumn(IMGItemImage, GridColumn);
            return IMGItemImage;
        }
        private Image CreateImage(Classes.Upgrade upgrade, int GridRow, int GridColumn)
        {
            Image IMGItemImage = new Image();
            IMGItemImage.HorizontalAlignment = HorizontalAlignment.Stretch;
            IMGItemImage.VerticalAlignment = VerticalAlignment.Stretch;
            IMGItemImage.Source = new BitmapImage(new Uri("ms-appx:///Assets/Square44x44Logo.scale-200.png"));//needs replacement
            IMGItemImage.Stretch = Stretch.Fill;
            Grid.SetRow(IMGItemImage, GridRow);
            Grid.SetColumn(IMGItemImage, GridColumn);
            return IMGItemImage;
        }
        private TextBlock CreateTextBlock(Classes.Item item, int GridRow, int GridColumn)
        {
            TextBlock TBItemInfo = new TextBlock();
            TBItemInfo.TextWrapping = TextWrapping.WrapWholeWords;
            TBItemInfo.HorizontalAlignment = HorizontalAlignment.Stretch;
            TBItemInfo.VerticalAlignment = VerticalAlignment.Stretch;
            TBItemInfo.Text = "Name: " + item.Name + "\nCost: " + item.Cost + "\nPower: " + item.Power + "\nCount: " + item.Count;
            TBItemInfo.FontSize = 13;
            Grid.SetRow(TBItemInfo, GridRow);
            Grid.SetColumn(TBItemInfo, GridColumn);
            return TBItemInfo;
        }
        private TextBlock CreateTextBlock(Classes.Upgrade upgrade, int GridRow, int GridColumn)
        {
            TextBlock TBItemInfo = new TextBlock();
            TBItemInfo.TextWrapping = TextWrapping.WrapWholeWords;
            TBItemInfo.HorizontalAlignment = HorizontalAlignment.Stretch;
            TBItemInfo.VerticalAlignment = VerticalAlignment.Stretch;
            TBItemInfo.Text = "Name: " + upgrade.Name + "\nCost: " + upgrade.Cost + "\nPower: " + upgrade.Power + "\nCount: " + upgrade.Count;
            TBItemInfo.FontSize = 13;
            Grid.SetRow(TBItemInfo, GridRow);
            Grid.SetColumn(TBItemInfo, GridColumn);
            return TBItemInfo;
        }
        private Button CreateButton(Classes.Item item, int GridRow, int GridColumn)
        {
            Button BTItemButton = new Button();
            BTItemButton.Content = "Click!";
            BTItemButton.HorizontalAlignment = HorizontalAlignment.Stretch;
            BTItemButton.VerticalAlignment = VerticalAlignment.Stretch;
            BTItemButton.Background = new SolidColorBrush(Colors.SkyBlue);
            BTItemButton.Click += (sender, e) => BTItemButton_Click(sender, e, item);
            BTItemButton.Tag = item.Cost;
            Grid.SetRow(BTItemButton, GridRow);
            Grid.SetColumn(BTItemButton, GridColumn);
            return BTItemButton;
        }
        private Button CreateButton(Classes.Upgrade upgrade, int GridRow, int GridColumn)
        {
            Button BTItemButton = new Button();
            BTItemButton.Content = "Click!";
            BTItemButton.HorizontalAlignment = HorizontalAlignment.Stretch;
            BTItemButton.VerticalAlignment = VerticalAlignment.Stretch;
            BTItemButton.Background = new SolidColorBrush(Colors.SkyBlue);
            BTItemButton.Click += (sender, e) => BTItemButton_Click(sender, e, upgrade);
            BTItemButton.Tag = upgrade.Cost;
            Grid.SetRow(BTItemButton, GridRow);
            Grid.SetColumn(BTItemButton, GridColumn);
            return BTItemButton;
        }
        private void BTItemButton_Click(object sender, RoutedEventArgs e, Classes.Item Item)
        {
            Button temp = sender as Button;
            if (User.Gold >= Item.Cost)
            {
                User.Gold -= Item.Cost;
                User.GoldPerSecond += Item.Power;
                Item.Count += 1;
                Item.Cost = Convert.ToSingle(Math.Round(Item.Cost * 1.15f));
                temp.Tag = Item.Cost;
                LoadItems(null, null);
            }
        }
        private void BTItemButton_Click(object sender, RoutedEventArgs e, Classes.Upgrade Upgrade)
        {
            Button temp = sender as Button;
            if (User.Gold >= Upgrade.Cost)
            {
                User.Gold -= Upgrade.Cost;
                User.GoldPerClick += Upgrade.Power;
                Upgrade.Count += 1;
                Upgrade.Cost = Convert.ToSingle(Math.Round(Upgrade.Cost * 1.15f));
                temp.Tag = Upgrade.Cost;
                LoadUpgrades(null, null);
            }
        }

    }
}
