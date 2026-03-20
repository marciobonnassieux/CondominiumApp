using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Condominium.Mobile.Models;
using System.Collections.ObjectModel;

namespace Condominium.Mobile.ViewModels;

public partial class ScanPageViewModel : ObservableObject
{
    [ObservableProperty]
    private bool _isProcessing;

    [ObservableProperty]
    private ImageSource? _capturedImage;

    public ObservableCollection<DetectedObject> DetectedObjects { get; } = new();

    public Action<string>? OnSelectionConfirmed;

    // Dimensões da imagem original obtida do hardware (essencial para mapear coordenadas)
    public double ImageWidth { get; private set; }
    public double ImageHeight { get; private set; }

    [RelayCommand]
    public async Task ProcessImageAsync(byte[] imageBytes)
    {
        IsProcessing = true;
        DetectedObjects.Clear();

        try
        {
            await Task.Run(async () =>
            {
                #if ANDROID
                // Engine Nativa de Visão Computacional (Expert level integration)
                using var bitmap = await Android.Graphics.BitmapFactory.DecodeByteArrayAsync(imageBytes, 0, imageBytes.Length);
                if (bitmap == null) return;

                ImageWidth = bitmap.Width;
                ImageHeight = bitmap.Height;

                using var inputImage = Xamarin.Google.MLKit.Vision.Common.InputImage.FromBitmap(bitmap, 0);

                // 1. Processar OCR (Texto)
                using var textRecognizer = Xamarin.GooglePlayServices.MLKit.Text.Recognition.TextRecognition.GetClient(
                    Xamarin.GooglePlayServices.MLKit.Text.Latin.TextRecognizerOptions.DefaultOptions);
                
                var textTask = textRecognizer.Process(inputImage);
                while (!textTask.IsComplete) await Task.Delay(10);
                
                if (textTask.IsSuccessful)
                {
                    var textResult = (Xamarin.GooglePlayServices.MLKit.Text.Text)textTask.Result;
                    foreach (var block in textResult.Blocks)
                    {
                        var rect = (Android.Graphics.Rect)block.BoundingBox;
                        if (rect == null) continue;

                        MainThread.BeginInvokeOnMainThread(() =>
                        {
                            DetectedObjects.Add(new DetectedObject
                            {
                                Value = block.Text,
                                Type = DetectionType.Text,
                                Bounds = new Rect(rect.Left, rect.Top, rect.Width(), rect.Height())
                            });
                        });
                    }
                }

                // 2. Processar Barcodes/QR
                using var barcodeScanner = Xamarin.GooglePlayServices.MLKit.Vision.Barcode.Scanning.BarcodeScanning.GetScanner();
                var barcodeTask = barcodeScanner.Process(inputImage);
                while (!barcodeTask.IsComplete) await Task.Delay(10);
                
                if (barcodeTask.IsSuccessful)
                {
                    var barcodeList = (Android.Runtime.JavaList)barcodeTask.Result;
                    foreach (var bcObj in barcodeList)
                    {
                        var bc = (Xamarin.GooglePlayServices.MLKit.Vision.Barcode.Common.Barcode)bcObj;
                        var rect = bc.BoundingBox;
                        if (rect == null) continue;

                        MainThread.BeginInvokeOnMainThread(() =>
                        {
                            DetectedObjects.Add(new DetectedObject
                            {
                                Value = bc.RawValue ?? bc.DisplayValue ?? "",
                                Type = bc.Format == Xamarin.GooglePlayServices.MLKit.Vision.Barcode.Common.Barcode.FormatQrCode 
                                       ? DetectionType.QrCode : DetectionType.Barcode,
                                Bounds = new Rect(rect.Left, rect.Top, rect.Width(), rect.Height())
                            });
                        });
                    }
                }

                bitmap.Recycle();
                #endif
            });
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Erro no processamento de visão: {ex.Message}");
        }
        finally
        {
            IsProcessing = false;
        }
    }

    public void SelectObject(DetectedObject obj)
    {
        OnSelectionConfirmed?.Invoke(obj.Value);
    }
}
