
namespace Frontend
{
    public partial class AppShell : Shell
    {

        public AppShell()
        {
            InitializeComponent();

        }

        public void ToggleAdminPageVisibility(bool isVisible)
        {
            AdminMenu.IsVisible = isVisible;
        }



    }

}

