using ZXing.Net.Maui;

namespace Condominium.Mobile.Pages;

public partial class ScanPage : ContentPage
{
    public Action<string>? OnCodeDetected;

    public ScanPage()
    {
        InitializeComponent();
        BarcodeReader.Options = new BarcodeReaderOptions
        {
            Formats = BarcodeFormats.All,
            AutoRotate = true,
            Multiple = false
        };
    }

    protected void BarcodesDetected(object sender, BarcodeDetectionEventArgs e)
    {
        var first = e.Results?.FirstOrDefault();
        if (first is not null)
        {
            Dispatcher.Dispatch(() =>
            {
                BarcodeReader.IsDetecting = false;
                OnCodeDetected?.Invoke(first.Value);
                Navigation.PopAsync();
            });
        }
    }
}
