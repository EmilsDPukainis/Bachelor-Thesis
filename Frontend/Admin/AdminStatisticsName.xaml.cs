using Newtonsoft.Json;
using Shared.Models;
using Shared.Other;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http.Json;
using UraniumUI.Pages;

namespace Frontend.Admin;

public partial class AdminStatisticsName : UraniumContentPage, INotifyPropertyChanged
{
    private string ApiBaseUrl => $"{BaseURL.APIBaseURL}";
    private readonly HttpClient _httpClient = new HttpClient();
    public ObservableCollection<CheckIns> CheckIns { get; set; }
    private int _userId;
    public int UserId
    {
        get { return _userId; }
        set
        {
            _userId = value;
            OnPropertyChanged(nameof(UserId));
        }
    }

    private string _userName;
    private string UserFullName;
    public string UserName
    {
        get { return _userName; }
        set
        {
            _userName = value;
            OnPropertyChanged(nameof(UserName));
        }
    }
    public event PropertyChangedEventHandler PropertyChanged;

    public AdminStatisticsName(int userId, string userName, string userFullName)
    {
        InitializeComponent();
        CheckIns = new ObservableCollection<CheckIns>();
        BindingContext = this; // Set the binding context to the current instance of AdminPage
        UserId = userId;
        UserName = userName;
        UserFullName = userFullName;

        LoadCheckInsAsync();
    }
    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);

        if (width > height)
        {
            // Landscape orientation
            if (Label1 != null)
            {
                stackLayout.Children.Remove(Label1);
                Label1 = null;
            }
        }
        else
        {
            // Portrait orientation
            if (Label1 == null)
            {
                Label1 = new Label
                {
                    Text = "For easier viewing, turn your device horizontally",
                    Margin = new Thickness(0, 10, 0, 10),
                    FontSize = 16,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center
                };
                stackLayout.Children.Insert(0, Label1); // Insert at index 0
            }

        }
    }
    private async Task LoadCheckInsAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync($"{ApiBaseUrl}/checkins/{_userId}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                CheckIns = JsonConvert.DeserializeObject<ObservableCollection<CheckIns>>(content);
                OnPropertyChanged(nameof(CheckIns));
            }
            else
            {
                await DisplayAlert("Error", "Failed to fetch check-in data", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
        }
    }


    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private async void BackButton_Clicked(object sender, EventArgs e)
    {
        // Navigate back to AdminStatistics page
        await Navigation.PopAsync();
    }
}

