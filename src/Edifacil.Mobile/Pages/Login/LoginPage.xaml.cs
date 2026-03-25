using System.Net.Http.Json;
using Edifacil.Mobile.Services;
using Edifacil.Mobile.Pages.Old;

namespace Edifacil.Mobile.Pages.Login;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
    }

    private async void OnLoginClicked(object? sender, EventArgs e)
    {
        var identifier = CpfEntry.Text?.Trim();
        var password = PasswordEntry.Text;

        if (string.IsNullOrWhiteSpace(identifier) || string.IsNullOrWhiteSpace(password))
        {
            await DisplayAlertAsync("Erro", "Preencha o UsuÃ¡rio e a Senha", "OK");
            return;
        }

        // Se o identificador nÃ£o for puramente digital (ex: tem @ ou .), enviamos como estÃ¡.
        // Se for puramente digital, poderÃ­amos formatar/limpar, mas para flexibilidade vamos enviar o que o usuÃ¡rio digitou.
        // A API agora aceita Email ou CPF no campo Identifier.

        LoadingIndicator.IsRunning = true;
        LoadingIndicator.IsVisible = true;

        try
        {
            var client = ApiService.GetClient();
            var response = await client.PostAsJsonAsync("/api/auth/login", new { Identifier = identifier, Password = password });

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
                await DisplayAlertAsync("Acesso Negado", "CPF ou senha invÃ¡lidos. Tente novamente.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Erro de Servidor", $"NÃ£o foi possÃ­vel conectar: {ex.Message}", "OK");
        }
        finally
        {
            LoadingIndicator.IsRunning = false;
        }
    }

    private void OnTogglePasswordClicked(object? sender, EventArgs e)
    {
        PasswordEntry.IsPassword = !PasswordEntry.IsPassword;
        // In a real app, we would also change the icon of TogglePasswordButton here.
    }

    private async void OnForgotPasswordTapped(object? sender, EventArgs e)
    {
        await DisplayAlertAsync("RecuperaÃ§Ã£o de Senha", "Funcionalidade de recuperaÃ§Ã£o de senha serÃ¡ implementada em breve.", "OK");
    }

    private async void OnCreateAccountTapped(object? sender, EventArgs e)
    {
        await DisplayAlertAsync("Criar Conta", "Funcionalidade de criaÃ§Ã£o de conta serÃ¡ implementada em breve.", "OK");
    }
}

public class LoginResponse
{
    public string Token { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}

