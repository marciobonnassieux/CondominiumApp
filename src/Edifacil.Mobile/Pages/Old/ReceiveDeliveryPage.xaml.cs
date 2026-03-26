using System.Collections.ObjectModel;
using System.Net.Http.Json;

namespace Edifacil.Mobile.Pages.Old;

public class RegisteredDelivery
{
    public string Code { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
}

public class SearchUnitItem
{
    public Guid Id { get; set; }
    public string Block { get; set; } = string.Empty;
    public string ApartmentNumber { get; set; } = string.Empty;
    public string DisplayText => $"Bloco {Block} - Apto {ApartmentNumber}";
}

public partial class ReceiveDeliveryPage : ContentPage
{
    public ObservableCollection<RegisteredDelivery> History { get; set; } = new();
    private List<SearchUnitItem> _allUnits = new();

    public ReceiveDeliveryPage()
    {
        InitializeComponent();
        RegisteredList.ItemsSource = History;
        LoadUnitsToComboBox();
    }

    private async void LoadUnitsToComboBox()
    {
        try
        {
            // Tentativa de obter as unidades cadastradas na base de dados conectada na rede
            var client = Services.ApiService.GetClient();
            var token = Preferences.Get("AuthToken", string.Empty);
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            
            var response = await client.GetAsync("/api/units");
            if (response.IsSuccessStatusCode)
            {
                var units = await response.Content.ReadFromJsonAsync<List<SearchUnitItem>>();
                if (units != null && units.Any())
                {
                    _allUnits = units;
                    OverlayUnitsList.ItemsSource = _allUnits;
                    return; // Retorna pois a API respondeu perfeitamente
                }
            }
        }
        catch { }

        // Fallback Mock local caso a API nÃ£o seja encontrada na rede WiFi
        _allUnits = new List<SearchUnitItem>
        {
            new() { Block = "A", ApartmentNumber = "101" },
            new() { Block = "A", ApartmentNumber = "102" },
            new() { Block = "B", ApartmentNumber = "201" },
            new() { Block = "B", ApartmentNumber = "202" },
            new() { Block = "C", ApartmentNumber = "301" }
        };
        OverlayUnitsList.ItemsSource = _allUnits;
    }

    // AÃ§Ãµes do Pop-Up (Combo-box Virtual)
    private void OnSearchUnitClicked(object? sender, EventArgs e)
    {
        UnitSearchBar.Text = string.Empty;
        OverlayUnitsList.ItemsSource = _allUnits;
        SearchOverlay.IsVisible = true;
    }

    private void OnSearchBarTextChanged(object? sender, TextChangedEventArgs e)
    {
        var keyword = e.NewTextValue?.ToLower() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(keyword))
            OverlayUnitsList.ItemsSource = _allUnits;
        else
            OverlayUnitsList.ItemsSource = _allUnits.Where(u => u.ApartmentNumber.Contains(keyword) || u.DisplayText.ToLower().Contains(keyword)).ToList();
    }

    private void OnOverlayUnitSelected(object? sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is SearchUnitItem selected)
        {
            UnitEntry.Text = selected.DisplayText;
            SearchOverlay.IsVisible = false;      
            OverlayUnitsList.SelectedItem = null; // reseta controle nativo
        }
    }

    private void OnCloseOverlayClicked(object? sender, EventArgs e)
    {
        SearchOverlay.IsVisible = false;
    }

    // Comportamentos originais de Encomendas
    private async void OnScanClicked(object? sender, EventArgs e)
    {
        var status = await Permissions.CheckStatusAsync<Permissions.Camera>();
        if (status != PermissionStatus.Granted)
            status = await Permissions.RequestAsync<Permissions.Camera>();

        if (status == PermissionStatus.Granted)
        {
            var scanPage = new ScanPage();
            scanPage.OnCodeDetected = (code) =>
            {
                BarcodeEntry.Text = code;
            };
            await Navigation.PushAsync(scanPage);
        }
        else
        {
            await DisplayAlert("Aviso", "A câmera recusa permissão nativa.", "OK");
        }
    }

    private async void OnRegisterClicked(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(UnitEntry.Text))
        {
            await DisplayAlert("Aviso", "Escolha na lupa de qual Unidade é este pacote.", "OK");
            return;
        }
        if (string.IsNullOrWhiteSpace(BarcodeEntry.Text))
        {
            await DisplayAlert("Aviso", "Faça o Scan da etiqueta primeiro.", "OK");
            return;
        }

        History.Insert(0, new RegisteredDelivery 
        { 
            Code = BarcodeEntry.Text, 
            Timestamp = DateTime.Now 
        });

        BarcodeEntry.Text = string.Empty; 
    }
}

