using Newtonsoft.Json;
using Shared.Models;
using Shared.Other;
using System.Collections.ObjectModel;
using System.ComponentModel;
using UraniumUI.Pages;

namespace Frontend;

public partial class AdminPage : UraniumContentPage, INotifyPropertyChanged
{

    public new event PropertyChangedEventHandler PropertyChanged;
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
    public AdminPage()
    {
        InitializeComponent();
       // BindingContext = this;
        //LoadUsers();
        
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        Shell.SetFlyoutBehavior(this, FlyoutBehavior.Flyout);
        BindingContext = this;

        LoadUsers();
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
                stackLayout.Children.Insert(0, Label1);
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
                stackLayout.Children.Insert(1, Label2);
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
                stackLayout.Children.Insert(2, Label3);
            }
        }
    }






    private async void LoadUsers()
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


    protected override void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
