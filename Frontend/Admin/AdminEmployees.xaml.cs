using Shared.Other;
using Shared.Models;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Frontend
{
    public partial class AdminEmployees : ContentPage
    {
        private string ApiBaseUrl => $"{BaseURL.APIBaseURL}";

        private readonly HttpClient _httpClient;

        public AdminEmployees()
        {
            InitializeComponent();
            _httpClient = new HttpClient();
            LoadEmployees();

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            Shell.SetFlyoutBehavior(this, FlyoutBehavior.Flyout);
            LoadEmployees();

        }




        private async void Remove_Employee(object sender, EventArgs e)
        {
            var selectedEmployeeName = Employee.SelectedItem as string;
            if (string.IsNullOrEmpty(selectedEmployeeName))
            {
                await DisplayAlert("Error", "Please select an employee to remove.", "OK");
                return;
            }

            try
            {
                // Fetch the user based on the selected name
                var user = await GetUserByName(selectedEmployeeName);
                if (user == null)
                {
                    await DisplayAlert("Error", $"User '{selectedEmployeeName}' not found.", "OK");
                    return;
                }
                else
                {
                    LoadEmployees();
                }

                // Use the fetched user's ID for deletion
                var response = await _httpClient.DeleteAsync($"{ApiBaseUrl}/users/{user.Id}");
                response.EnsureSuccessStatusCode();

                await DisplayAlert("Success", $"Employee '{selectedEmployeeName}' removed successfully.", "OK");

                LoadEmployees(); // Reload employees after deletion
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to remove employee: {ex.Message}", "OK");
            }
        }

        private async Task<User> GetUserByName(string name)
        {
            try
            {
                // Encode the name parameter before appending it to the URL
                string encodedName = Uri.EscapeDataString(name);
                var response = await _httpClient.GetAsync($"{ApiBaseUrl}/users?Name={encodedName}");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var users = JsonConvert.DeserializeObject<List<User>>(content);

                // Filter users by name and return the first matching user
                return users.FirstOrDefault(u => u.Name == name);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to fetch user by name: {ex.Message}");
            }
        }



        private async void Reset_Employees(object sender, EventArgs e)
        {
            try
            {
                var response = await _httpClient.PostAsync($"{ApiBaseUrl}/users/reset", null);
                response.EnsureSuccessStatusCode();

                await DisplayAlert("Success", "All employees reset successfully.", "OK");
                LoadEmployees();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to reset employees: {ex.Message}", "OK");
            }
        }

        private async void LoadEmployees()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{ApiBaseUrl}/users");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var users = JsonConvert.DeserializeObject<List<User>>(content);

                if (users != null && users.Any())
                {
                    Employee.ItemsSource = users.Select(u => u.Name).ToList();
                }

            }
            catch (HttpRequestException ex)
            {
                await DisplayAlert("Error", $"Failed to retrieve employees: {ex.Message}", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }
    }
}
    