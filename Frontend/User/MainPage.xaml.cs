    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Json;
    using System.Text;
    using System.Threading.Tasks;
using Frontend.Services;
using Microsoft.Maui.Controls;
using Newtonsoft.Json;
using Shared.Models;
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



    private async void Submit_Code(object sender, EventArgs e)
        {
            if (Entry == null || string.IsNullOrWhiteSpace(Entry.Text))
            {
                await DisplayAlert("Error", "Please enter a code.", "OK");
                return;
        }
        string code = Entry.Text.Trim();


        if (IsAdminCode(code))
        {
            // Handle admin access separately (e.g., show AdminPage)
            HandleAdminAccess();
            Entry.Text = string.Empty; // Clear the entry after processing
        }
        else
        {
            try
            {
                // Proceed with regular check-in/out for non-admin users
            var (latitude, longitude) = await GetLocationAsync();
                double workLatitude = 56.91081106204646;
                double workLongitude = 24.0805702852543;

                // Check if the user is on site or remote
                bool isOnSite = _locationhelper.IsWithinRadius(latitude, longitude, workLatitude, workLongitude, 0.2); // 200 meter radius
                string location = isOnSite ? "OnSite" : "Remote";

                // Proceed with regular check-in/out for non-admin users
                var response = await CheckInUserAsync(code, location);

                if (response.IsSuccessStatusCode)
                {
                    // Read response content
                    string responseContent = await response.Content.ReadAsStringAsync();

                    // Display success message from controller
                    await DisplayAlert("Success", responseContent, "OK");

                    Entry.Text = string.Empty;
                }
                else
                {
                    // Read error message from response content
                    string errorMessage = await response.Content.ReadAsStringAsync();

                    // Display error message from controller
                    await DisplayAlert("Error", errorMessage, "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }
    }

    private bool IsAdminCode(string code)
    {
        string AdminCode = "1111";

        return code == AdminCode;
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

            // Call the API endpoint to check-in/out the user
            return await client.PostAsync($"{ApiBaseUrl}/checkins", content);
        }
    }

    public async Task<(double Latitude, double Longitude)> GetLocationAsync()
    {
        try
        {
            // Request permission to access location
            var status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();

            // Check if permission is granted
            if (status == PermissionStatus.Granted)
            {
                // Get the device's current location
                var location = await Geolocation.GetLocationAsync(new GeolocationRequest
                {
                    DesiredAccuracy = GeolocationAccuracy.Medium, // Adjust accuracy as needed
                    Timeout = TimeSpan.FromSeconds(10) // Set timeout for location request
                });

                if (location != null)
                {
                    // Return the latitude and longitude
                    return (location.Latitude, location.Longitude);
                }
                else
                {
                    // Location services are enabled but unable to retrieve location
                    return (0.0, 0.0);
                }
            }
            else
            {
                // Location permission not granted
                return (0.0, 0.0);
            }
        }
        catch (Exception ex)
        {
            // Handle location service errors
            Console.WriteLine($"Error getting location: {ex.Message}");
            return (0.0, 0.0);
        }
    }
   private void SetGreetingMessage()
        {
            // Get the current time
            DateTime currentTime = DateTime.Now;

            // Check the current time and set the greeting message accordingly
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

            // Update the text of the label to display the greeting message
            Hello.Text = greetingMessage;
        }

    
}
