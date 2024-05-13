using Microsoft.Maui.Graphics;
using Newtonsoft.Json;
using Shared.Models;
using Shared.Other;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.Json;
using UraniumUI.Pages;

namespace Frontend;

public partial class AdminPage : UraniumContentPage
{

    public event PropertyChangedEventHandler PropertyChanged;
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

    private string ApiBaseUrl => $"{BaseURL.APIBaseURL}";

    private readonly HttpClient _httpClient = new HttpClient();
    public ObservableCollection<User> User { get; set; }
    public AdminPage()
	{
		InitializeComponent();

        User = new ObservableCollection<User>();

        BindingContext = this; // Set the binding context to the current instance of AdminPage
        LoadUsers();

    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        Shell.SetFlyoutBehavior(this, FlyoutBehavior.Flyout);
        if (DataGrid != null)
        {
            // Call the method to load users
            LoadUsers();
        }

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
            if (Label2 != null)
            {
                stackLayout.Children.Remove(Label2);
                Label2 = null;
            }
            if (Label3 != null)
            {
                stackLayout.Children.Remove(Label3);
                Label3 = null;
            }
        }
        else
        {
            // Portrait orientation
            if (Label1 == null)
            {
                Label1 = new Label
                {
                    Text = "Welcome to Administration",
                    Margin = new Thickness(0, 20, 0, 0),
                    FontSize = 26,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center
                };
                stackLayout.Children.Insert(0, Label1); // Insert at index 0
            }

            if (Label2 == null)
            {
                Label2 = new Label
                {
                    Text = "Here you can view registered employees",
                    Margin = new Thickness(0, 10, 0, 0),
                    FontSize = 16,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center
                };
                stackLayout.Children.Insert(1, Label2); // Insert at index 1
            }
            if (Label3 == null)
            {
                Label3 = new Label
                {
                    Text = "For easier viewing, turn your device horizontally",
                    Margin = new Thickness(0, 0, 0, 10),
                    FontSize = 16,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center
                };
                stackLayout.Children.Insert(2, Label3); // Insert at index 1
            }
        }
    }






    private async Task LoadUsers()
    {
        try
        {
            var response = await _httpClient.GetAsync($"{ApiBaseUrl}/users");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var users = JsonConvert.DeserializeObject<ObservableCollection<User>>(content);

            DataGrid.ItemsSource = users;
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
}
