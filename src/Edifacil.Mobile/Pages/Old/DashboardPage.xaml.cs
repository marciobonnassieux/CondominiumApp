namespace Edifacil.Mobile.Pages.Old;

public partial class DashboardPage : ContentPage
{
    public DashboardPage()
    {
        InitializeComponent();
    }

    private async void OnViewUnitsClicked(object? sender, EventArgs e)
    {
        await Navigation.PushAsync(new UnitsPage());
    }

    private async void OnReceiveDeliveryClicked(object? sender, EventArgs e)
    {
        await Navigation.PushAsync(new ReceiveDeliveryPage());
    }
}

