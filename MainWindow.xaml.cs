using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Windows;
using System.Windows.Media.Imaging;

namespace RandomCoffee
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly HttpClient client = new HttpClient();

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            using HttpResponseMessage response = await client.GetAsync("https://coffee.alexflipnote.dev/random.json", HttpCompletionOption.ResponseHeadersRead);

            response.EnsureSuccessStatusCode();

            if (response.Content is object && response.Content.Headers.ContentType!.MediaType == "application/json")
            {
                Stream contentStream = await response.Content.ReadAsStreamAsync();

                using StreamReader streamReader = new StreamReader(contentStream);
                using JsonTextReader jsonReader = new JsonTextReader(streamReader);

                JsonSerializer serializer = new JsonSerializer();
                Coffee coffee = serializer.Deserialize<Coffee>(jsonReader)!;

                Uri coffeeUri = new Uri(coffee.File);
                this.CoffeeImage.Source = BitmapFrame.Create(coffeeUri);
            }
            else
            {
                throw new Exception("HTTP Response was invalid and cannot be deserialised.");
            }
        }
    }
}
