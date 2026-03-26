using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Edifacil.Mobile.ViewModels;

public partial class LogoutConfirmationViewModel : ObservableObject
{
    // Mock do serviço de autenticação
    [RelayCommand]
    private async Task ConfirmLogoutAsync()
    {
        // Aqui iria a chamada ao IAuthService.Logout()
        await Shell.Current.GoToAsync("//LoginPage");
    }

    [RelayCommand]
    private async Task CancelAsync()
    {
        // Fecha o popup ou volta
        await Shell.Current.Navigation.PopModalAsync();
    }
}
