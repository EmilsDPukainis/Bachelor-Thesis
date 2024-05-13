namespace Frontend;

public partial class AdminTutorial : ContentPage
{
	public AdminTutorial()
	{
		InitializeComponent();
	}
    protected override void OnAppearing()
    {
        base.OnAppearing();

        Shell.SetFlyoutBehavior(this, FlyoutBehavior.Flyout);
    }
}