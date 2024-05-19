using Shared.Other;
using System.Text.Json;

namespace Frontend;

public partial class AdminJobs : ContentPage
{
    private readonly HttpClient _httpClient;
    private string ApiBaseUrl => $"{BaseURL.APIBaseURL}";

    public AdminJobs()
    {
        InitializeComponent();
        _httpClient = new HttpClient();
        LoadJobs();
    }



    protected override void OnAppearing()
    {
        base.OnAppearing();
        LoadJobs();

        Shell.SetFlyoutBehavior(this, FlyoutBehavior.Flyout);
    }

    private async void LoadJobs()
    {
        try
        {
            var response = await _httpClient.GetAsync($"{ApiBaseUrl}/jobs");
            response.EnsureSuccessStatusCode();

            var jobsJson = await response.Content.ReadAsStringAsync();
            var jobs = JsonSerializer.Deserialize<List<string>>(jobsJson);

            Job.ItemsSource = jobs;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to load jobs: {ex.Message}", "OK");
        }
    }

    private async void Remove_Job(object sender, EventArgs e)
    {
        var selectedJob = Job.SelectedItem as string;
        if (string.IsNullOrWhiteSpace(selectedJob))
        {
            await DisplayAlert("Error", "Please select a job to remove.", "OK");
            return;
        }

        try
        {

            var response = await _httpClient.DeleteAsync($"{ApiBaseUrl}/jobs/{selectedJob}");
            response.EnsureSuccessStatusCode();

            await DisplayAlert("Success", "Job removed successfully.", "OK");
            LoadJobs();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to remove job: {ex.Message}", "OK");
        }
    }

    private async void Add_Job(object sender, EventArgs e)
    {
        try
        {
            var newJobTitle = JobEntry.Text.Trim();

            if (string.IsNullOrWhiteSpace(newJobTitle))
            {
                await DisplayAlert("Error", "Please enter a job title.", "OK");
                return;
            }

            var jobExistsResponse = await _httpClient.GetAsync($"{ApiBaseUrl}/jobs/check?title={Uri.EscapeDataString(newJobTitle)}");
            if (jobExistsResponse.IsSuccessStatusCode)
            {
                await DisplayAlert("Error", $"Job '{newJobTitle}' already exists.", "OK");
                return;
            }

            var content = new StringContent(JsonSerializer.Serialize(newJobTitle), System.Text.Encoding.UTF8, "application/json");
            var addJobResponse = await _httpClient.PostAsync($"{ApiBaseUrl}/jobs", content);

            if (addJobResponse.IsSuccessStatusCode)
            {
                await DisplayAlert("Success", "Job added successfully.", "OK");
                JobEntry.Text = string.Empty;
                LoadJobs();
            }
            else
            {
                await DisplayAlert("Error", "This job has already been made", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
        }
    }

    private async void Reset_Jobs(object sender, EventArgs e)
    {
        try
        {
            var response = await _httpClient.PostAsync($"{ApiBaseUrl}/jobs/reset", null);
            response.EnsureSuccessStatusCode();

            await DisplayAlert("Success", "Jobs reset successfully.", "OK");
            LoadJobs();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to reset jobs: {ex.Message}", "OK");
        }
    }
}

