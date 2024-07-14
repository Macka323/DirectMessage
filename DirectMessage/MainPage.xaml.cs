using DirectMessage.Helper;
using DirectMessage.Model;
using System.Text.Json;

namespace DirectMessage
{
    public partial class MainPage : ContentPage
    {


        public MainPage()
        {
            InitializeComponent();
            PhoneEntery.Loaded += PhoneEntery_Loaded;
            CountryPicker.Loaded += CountryPicker_Loaded;
            CountryPicker.SelectedIndexChanged += CountryPicker_SelectedIndexChanged;
        }

        private async void CountryPicker_SelectedIndexChanged(object? sender, EventArgs e)
        {
            using var stream = await FileSystem.OpenAppPackageFileAsync("PhoneFormats.json");
            using var reader = new StreamReader(stream);

            var contents = reader.ReadToEnd();

            var startIndex = contents.IndexOf(CountryPicker.SelectedItem.ToString().Split(' ')[0]);

            var format = contents.Substring(startIndex + CountryPicker.SelectedItem.ToString().Split(' ')[0].Length
                , contents.IndexOf(',', startIndex) - startIndex - 5);

            PhoneEntery.Placeholder = format.Replace("-", " ");

            DirectMessageConfig.SetCountryCode(CountryPicker.SelectedItem.ToString());
        }

        private async void CountryPicker_Loaded(object? sender, EventArgs e)
        {
            using var stream = await FileSystem.OpenAppPackageFileAsync("countries.json");
            using var reader = new StreamReader(stream);

            var contents = reader.ReadToEnd();


            List<Country>? countries = JsonSerializer.Deserialize<List<Country>>(contents);

            foreach (var c in countries)
            {

                CountryPicker.Items.Add(c.dial_code + "  " + c.name);
            }

            CountryPicker.SelectedItem = DirectMessageConfig.GetCountryCode();

        }

        private void PhoneEntery_Loaded(object? sender, EventArgs e)
        {
            PhoneEntery.Focus();
        }
        async Task<bool> checkNumberLenght()
        {
            int numbers = 0;
            foreach (var c in PhoneEntery.Placeholder)
                if (c == '#')
                    numbers++;

            if (numbers != PhoneEntery.Text.Length)
            {
                await DisplayAlert("Check the phone number", "", "ok");
                return false;
            }
            return true;
        }

        private async void Telegram_Clicked(object sender, EventArgs e)
        {
            // DirectMessageConfig.AddPrevNumbers(CountryPicker.SelectedItem.ToString().Split(' ')[0] + PhoneEntery.Text);

            if (!await checkNumberLenght())
                return;

            try
            {
                Uri uri = new Uri("https://www.microsoft.com");
                BrowserLaunchOptions options = new BrowserLaunchOptions()
                {
                    LaunchMode = BrowserLaunchMode.SystemPreferred,
                    TitleMode = BrowserTitleMode.Hide,
                    PreferredToolbarColor = Colors.Violet,
                    PreferredControlColor = Colors.SandyBrown

                };

                await Browser.Default.OpenAsync(uri, options);
            }
            catch (Exception ex)
            {
                // An unexpected error occurred. No browser may be installed on the device.
            }
        }

        private async void Viber_Clicked(object sender, EventArgs e)
        {
            if (!await checkNumberLenght())
                return;
            try
            {
                Uri uri = new Uri($"viber://chat?number={CountryPicker.SelectedItem.ToString().Split(' ')[0].Trim('+')+PhoneEntery.Text}");
                BrowserLaunchOptions options = new BrowserLaunchOptions()
                {
                    LaunchMode = BrowserLaunchMode.SystemPreferred,
                    TitleMode = BrowserTitleMode.Hide,
                    PreferredToolbarColor = Colors.Violet,
                    PreferredControlColor = Colors.SandyBrown

                };

                await Browser.Default.OpenAsync(uri, options);
            }
            catch (Exception ex)
            {
                // An unexpected error occurred. No browser may be installed on the device.
            }
        }

        private async void WhatsApp_Clicked(object sender, EventArgs e)
        {
            if (!await checkNumberLenght())
                return;
            try
            {
                Uri uri = new Uri($"https://api.whatsapp.com/send?phone={PhoneEntery.Text}");
                BrowserLaunchOptions options = new BrowserLaunchOptions()
                {
                    LaunchMode = BrowserLaunchMode.SystemPreferred,
                    TitleMode = BrowserTitleMode.Hide,
                    PreferredToolbarColor = Colors.Violet,
                    PreferredControlColor = Colors.SandyBrown

                };

                await Browser.Default.OpenAsync(uri, options);
            }
            catch (Exception ex)
            {
                // An unexpected error occurred. No browser may be installed on the device.
            }
        }


    }

}
