using System.Net.Http.Headers;
using System.Net.Http.Json;
using Edifacil.Mobile.Services;

namespace Edifacil.Mobile.Pages.Old;

public partial class UnitsPage : ContentPage
{
    public UnitsPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadUnits();
    }

    private async void OnRefreshing(object? sender, EventArgs e)
    {
        await LoadUnits();
        UnitsRefreshView.IsRefreshing = false;
    }

    private async Task LoadUnits()
    {
        try
        {
            var token = Preferences.Get("AuthToken", string.Empty);
            var client = ApiService.GetClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync("/api/units");

            if (response.IsSuccessStatusCode)
            {
                var units = await response.Content.ReadFromJsonAsync<List<UnitDto>>();
                UnitsCollection.ItemsSource = units;
            }
            else
            {
                await DisplayAlertAsync("Aviso", "Acesso Negado: VocÃª nÃ£o estÃ¡ logado ou o token expirou.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Erro", "Falha de rede ao buscar as unidades: " + ex.Message, "OK");
        }
    }
}

public class UnitDto
{
    public Guid Id { get; set; }
    public string Block { get; set; } = string.Empty;
    public string ApartmentNumber { get; set; } = string.Empty;
}

