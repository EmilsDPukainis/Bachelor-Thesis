    using System.Text;
using Frontend.Services;
using Newtonsoft.Json;
    using Shared.Other;

    namespace Frontend;
    public partial class MainPage : ContentPage
    {
    private string ApiBaseUrl => $"{BaseURL.APIBaseURL}";
    private LocationHelper _locationhelper;

    public MainPage()
        {
            InitializeComponent();
            SetGreetingMessage();
            _locationhelper = new LocationHelper();

    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        SetGreetingMessage();
    }

    private async void Submit_Code(object sender, EventArgs e)
        {
            if (Entry == null || string.IsNullOrWhiteSpace(Entry.Text))
            {
                await DisplayAlert("Error", "Please enter a code.", "OK");
                return;
        }
        string code = Entry.Text.Trim();


        if (AdminCode.IsAdminCode(code))
        {
            HandleAdminAccess();
            Entry.Text = string.Empty; 
        }
        else
        {
            try
            {
            var (latitude, longitude) = await GetLocationAsync();
                double workLatitude = 56.91081106204646;
                double workLongitude = 24.0805702852543;

                bool isOnSite = _locationhelper.IsWithinRadius(latitude, longitude, workLatitude, workLongitude, 0.2);
                string location = isOnSite ? "OnSite" : "Remote";

                var response = await CheckInUserAsync(code, location);

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();

                    await DisplayAlert("Success", responseContent, "OK");

                    Entry.Text = string.Empty;
                }
                else
                {
                    string errorMessage = await response.Content.ReadAsStringAsync();

                    await DisplayAlert("Error", errorMessage, "OK");
                    Entry.Text = string.Empty;

                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }
    }

    private async void HandleAdminAccess()
    {

        await Shell.Current.GoToAsync("//AdminPage");
    }

    private async Task<HttpResponseMessage> CheckInUserAsync(string code, string location)
    {
        using (var client = new HttpClient())
        {
            var requestData = new
            {
                code,
                location
            };

            var jsonContent = JsonConvert.SerializeObject(requestData);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            return await client.PostAsync($"{ApiBaseUrl}/checkins", content);
        }
    }

    public async Task<(double Latitude, double Longitude)> GetLocationAsync()
    {
        try
        {
            PermissionStatus status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            }

            if (status == PermissionStatus.Granted)
            {
                // Get the device's current location
                var location = await Geolocation.GetLocationAsync(new GeolocationRequest
                {
                    DesiredAccuracy = GeolocationAccuracy.Medium, 
                    Timeout = TimeSpan.FromSeconds(10)
                });

                if (location != null)
                {
                    // Return the latitude and longitude
                    return (location.Latitude, location.Longitude);
                }
                else
                {
                    return (1.1, 1.1);
                    //throw new Exception("Unable to retrieve location");
                }
            }
            else

            {
                return (1.1, 1.1);

                //throw new Exception("To continue, please enable location permissions");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting location: {ex.Message}");
            throw;
        }
    }
   private void SetGreetingMessage()
        {
            DateTime currentTime = DateTime.Now;

            string greetingMessage = "";
            if (currentTime.Hour < 11)
            {
                greetingMessage = "Good Morning";
            }
            else if (currentTime.Hour < 18)
            {
                greetingMessage = "Good Day";
            }
            else
            {
                greetingMessage = "Good Evening";
            }

            Hello.Text = greetingMessage;
        }
}
