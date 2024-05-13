using Frontend.Admin;
using Newtonsoft.Json;
using Shared.Models;
using Shared.Other;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using UraniumUI.Pages;

namespace Frontend;

public partial class AdminStatistics : UraniumContentPage, INotifyPropertyChanged
{
    private string ApiBaseUrl => $"{BaseURL.APIBaseURL}";
    private readonly HttpClient _httpClient = new HttpClient();

    public ICommand EmployeeButtonCommand { get; }

    private ObservableCollection<User> _users;
    public ObservableCollection<User> Users
    {
        get => _users;
        set
        {
            _users = value;
            OnPropertyChanged(nameof(Users));
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    public AdminStatistics()
    {
        InitializeComponent();
        Users = new ObservableCollection<User>();
        EmployeeButtonCommand = new Command<User>(EmployeeButton_Clicked);

        BindingContext = this; // Set the binding context to the current instance of AdminPage
        LoadUsersAsync();
    }


    protected override void OnAppearing()
    {
        base.OnAppearing();

        Shell.SetFlyoutBehavior(this, FlyoutBehavior.Flyout);
        LoadUsersAsync();
    }


    private async Task LoadUsersAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync($"{ApiBaseUrl}/users");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var users = JsonConvert.DeserializeObject<List<User>>(content);

            Device.BeginInvokeOnMainThread(() =>
            {
                Users = new ObservableCollection<User>(users);
            });
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to load users: {ex.Message}", "OK");
        }
    }

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

private async void EmployeeButton_Clicked(User user)
{
    // Extract the user ID and name
    var userId = user.Id;
    var userName = user.Name;
    var userFullName = user.FullName;

    // Navigate to AdminStatisticsName page, passing userId and userName as parameters
    await Navigation.PushAsync(new AdminStatisticsName(userId, userName, userFullName));
}
}

