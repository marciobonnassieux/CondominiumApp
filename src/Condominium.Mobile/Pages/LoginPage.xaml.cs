using System.Net.Http.Json;
using Condominium.Mobile.Services;

namespace Condominium.Mobile.Pages;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
    }

    private async void OnLoginClicked(object? sender, EventArgs e)
    {
        // Envia apenas os números em texto puro para a API
        var cpf = new string(CpfEntry.Text?.Where(char.IsDigit).ToArray() ?? Array.Empty<char>());
        var password = PasswordEntry.Text;

        if (string.IsNullOrWhiteSpace(cpf) || string.IsNullOrWhiteSpace(password))
        {
            await DisplayAlertAsync("Erro", "Preencha o CPF e a Senha", "OK");
            return;
        }

        LoadingIndicator.IsRunning = true;
        LoadingIndicator.IsVisible = true;

        try
        {
            var client = ApiService.GetClient();
            var response = await client.PostAsJsonAsync("/api/auth/login", new { Cpf = cpf, Password = password });

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
                // Salva o JWT no armazenamento do celular
                Preferences.Set("AuthToken", result?.Token);
                
                // Navega para a tela principal (Dashboard)
                await Navigation.PushAsync(new DashboardPage());
            }
            else
            {
                await DisplayAlertAsync("Acesso Negado", "CPF ou senha inválidos. Tente novamente.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Erro de Servidor", $"Não foi possível conectar: {ex.Message}", "OK");
        }
        finally
        {
            LoadingIndicator.IsRunning = false;
            LoadingIndicator.IsVisible = false;
        }
    }
}

public class LoginResponse
{
    public string Token { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}
