using Newtonsoft.Json;
using Shared.Models;
using Shared.Other;
using System.Collections.ObjectModel;
using System.Text;

namespace Frontend;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class RegisterPage : ContentPage
{
    private readonly HttpClient _httpClient = new HttpClient();
    private string ApiBaseUrl => $"{BaseURL.APIBaseURL}";
    public ObservableCollection<string> Jobs { get; set; } = new ObservableCollection<string>();
    public bool IsAdminPage { get; } = false; // For non-admin pages

    public RegisterPage()
    {
        InitializeComponent();
        BindingContext = this; // Set the BindingContext to the current page instance
        InitializeAsync(); // Call an async method to perform initialization

    }
    private async void InitializeAsync()
    {
        await LoadJobs(); // Await the LoadJobs method
    }

    private async Task LoadJobs()
    {
        try
        {
            var response = await _httpClient.GetAsync($"{ApiBaseUrl}/jobs");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var jobOptions = JsonConvert.DeserializeObject<List<string>>(json);

                Jobs.Clear();
                foreach (var job in jobOptions)
                {
                    Jobs.Add(job);
                }
            }
            else
            {
                await DisplayAlert("Error", $"Failed to load job options: {response.ReasonPhrase}", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
        }
    }

    private async void Submit_Button(object sender, EventArgs e)
    {
        string selectedJob = Job.SelectedItem as string;

        if (string.IsNullOrEmpty(selectedJob))
        {
            await DisplayAlert("Error", "Please select a valid job.", "OK");
            return;
        }

        string firstName = FirstName.Text;
        string lastName = LastName.Text;

        // Validate first name, last name, and selected job
        if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
        {
            await DisplayAlert("Error", "Please fill in all fields", "OK");
            return;
        }
        if (!IsAllAlphabetic(firstName) || !IsAllAlphabetic(lastName))
        {
            await DisplayAlert("Error", "Name and surname must contain only alphabetical characters.", "OK");
            return;
        }
        const int maxNameLength = 20;
        if (firstName.Length > maxNameLength || lastName.Length > maxNameLength)
        {
            await DisplayAlert("Error", $"Name and surname must be at most {maxNameLength} characters long.", "OK");
            return;
        }

        var registrationModel = new User
        {
            Name = firstName,
            Surname = lastName,
            Job = selectedJob,
            Time = DateTime.Now.ToString("HH:mm"), // Set current time in 24-hour format
            Date = DateTime.Today.ToString("dd-MM-yyyy") // Set current date in dd-MM-yyyy format

    };

        string json = JsonConvert.SerializeObject(registrationModel);

        try
        {
            var response = await _httpClient.PostAsync($"{ApiBaseUrl}/users", new StringContent(json, Encoding.UTF8, "application/json"));

            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                var responseObject = JsonConvert.DeserializeObject<dynamic>(responseContent);

                await DisplayAlert("Success", $"Registration successful!\nYour Code Is: {responseObject.registrationCode}\nSave it, as you won't see it anymore", "OK");

                // Reset form after successful submission
                FirstName.Text = string.Empty;
                LastName.Text = string.Empty;
                Job.SelectedItem = null; // Clear job selection
            }
            else
            {
                await DisplayAlert("Error", $"Registration failed: {response.ReasonPhrase}", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
        }
    }

    private bool IsAllAlphabetic(string value)
    {
        return value.All(char.IsLetter);
    }
}
