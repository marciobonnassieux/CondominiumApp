using Microsoft.Maui.Graphics;
using Condominium.Mobile.Models;
using Condominium.Mobile.ViewModels;
using System.Linq;

namespace Condominium.Mobile.Pages;

public class PhotoScannerDrawable : IDrawable
{
    public required ScanPageViewModel ViewModel { get; set; }

    public void Draw(ICanvas canvas, RectF viewRect)
    {
        if (ViewModel == null || !ViewModel.DetectedObjects.Any())
            return;

        // Cálculo de Matriz AspectFit (Crucial para expert mapping)
        double imgW = ViewModel.ImageWidth;
        double imgH = ViewModel.ImageHeight;
        double viewW = viewRect.Width;
        double viewH = viewRect.Height;

        if (imgW <= 0 || imgH <= 0 || viewW <= 0 || viewH <= 0) return;

        double scale = Math.Min(viewW / imgW, viewH / imgH);
        float offsetX = (float)((viewW - (imgW * scale)) / 2.0);
        float offsetY = (float)((viewH - (imgH * scale)) / 2.0);

        foreach (var obj in ViewModel.DetectedObjects)
        {
            canvas.StrokeColor = obj.GetDisplayColor();
            canvas.StrokeSize = 4;
            canvas.FillColor = obj.GetDisplayColor().WithAlpha(0.2f);

            // Mapeia coordenadas da imagem original para coordenadas da tela MAUI
            float x = (float)(obj.Bounds.X * scale) + offsetX;
            float y = (float)(obj.Bounds.Y * scale) + offsetY;
            float w = (float)(obj.Bounds.Width * scale);
            float h = (float)(obj.Bounds.Height * scale);

            canvas.DrawRoundedRectangle(x, y, w, h, 8);
            canvas.FillRoundedRectangle(x, y, w, h, 8);
        }
    }
}

public partial class ScanPage : ContentPage
{
    public Action<string>? OnCodeDetected;
    private ScanPageViewModel _viewModel;
    private PhotoScannerDrawable _drawable;

    private double _currentScale = 1;
    private double _startScale = 1;
    private double _xOffset = 0;
    private double _yOffset = 0;

    public ScanPage()
    {
        InitializeComponent();
        _viewModel = new ScanPageViewModel();
        BindingContext = _viewModel;
        
        _drawable = new PhotoScannerDrawable { ViewModel = _viewModel };
        ArOverlay.Drawable = _drawable;

        _viewModel.OnSelectionConfirmed = (val) =>
        {
            OnCodeDetected?.Invoke(val);
        };
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        
        // Aguarda permissão e inicializa câmera (Expert safe check)
        var status = await Permissions.CheckStatusAsync<Permissions.Camera>();
        if (status != PermissionStatus.Granted)
            status = await Permissions.RequestAsync<Permissions.Camera>();

        if (status == PermissionStatus.Granted)
        {
            MainThread.BeginInvokeOnMainThread(async () => {
                // Força o trigger de carregamento da câmera no Grid
                await Task.Delay(500);
                if (CameraControl.Cameras.Count > 0)
                {
                    CameraControl.Camera = CameraControl.Cameras.First();
                    CameraControl.AutoSnapShotAsImageSource = true;
                    // Trigger de start in-page
                    var result = await CameraControl.StartCameraAsync();
                }
            });
        }
    }

    private async void OnCaptureClicked(object sender, EventArgs e)
    {
        // 1. Capturar Bitmap/Frame de Alta Resolução de dentro da Lente
        // Camera.MAUI 1.5.1 TakePhotoAsync retorna Stream, precisa dispose
        byte[] imageBytes = null;
        using (var stream = await CameraControl.TakePhotoAsync())
        {
            if (stream == null) return;

            // 2. Mudar Estados Visuais
            CameraGrid.IsVisible = false;
            ReviewGrid.IsVisible = true;
            
            using (var ms = new MemoryStream())
            {
                await stream.CopyToAsync(ms);
                ms.Position = 0;
                imageBytes = ms.ToArray();
            }
        }
        
        FrozenImage.Source = ImageSource.FromStream(() => new MemoryStream(imageBytes));

        // 3. Processar Visão (ViewModel)
        if (imageBytes != null)
        {
            // 4. Delegar para a ViewModel disparar o Google ML Kit (Task.Run interno)
            await _viewModel.ProcessImageAsync(imageBytes);

            // 5. Forçar Redesenho do GraphicsView sobre a foto
            ArOverlay.Invalidate();
        }
    }

    private void OnRetryClicked(object sender, EventArgs e)
    {
        // Limpar imagem para liberar memória
        FrozenImage.Source = null;
        
        // Reset do Zoom e Pan
        _currentScale = 1;
        _xOffset = 0;
        _yOffset = 0;
        ZoomableContainer.Scale = 1;
        ZoomableContainer.TranslationX = 0;
        ZoomableContainer.TranslationY = 0;

        ReviewGrid.IsVisible = false;
        CameraGrid.IsVisible = true;
        _viewModel.DetectedObjects.Clear();
        ArOverlay.Invalidate();
    }

    private async void OnOverlayTapped(object sender, TappedEventArgs e)
    {
        var touchPos = e.GetPosition(ArOverlay);
        if (touchPos == null || !_viewModel.DetectedObjects.Any()) return;

        double touchX = touchPos.Value.X;
        double touchY = touchPos.Value.Y;

        // Hit Testing Matemático usando a mesma lógica do Drawable (Inversive Mapping)
        double imgW = _viewModel.ImageWidth;
        double imgH = _viewModel.ImageHeight;
        double viewW = ArOverlay.Width;
        double viewH = ArOverlay.Height;

        double scale = Math.Min(viewW / imgW, viewH / imgH);
        double offsetX = (viewW - (imgW * scale)) / 2.0;
        double offsetY = (viewH - (imgH * scale)) / 2.0;

        foreach (var obj in _viewModel.DetectedObjects)
        {
            // Transforma o Bounds (Image Space) para Screen Space para o check
            double x = (obj.Bounds.X * scale) + offsetX;
            double y = (obj.Bounds.Y * scale) + offsetY;
            double w = (obj.Bounds.Width * scale);
            double h = (obj.Bounds.Height * scale);

            if (touchX >= x && touchX <= x + w && touchY >= y && touchY <= y + h)
            {
                // Feedback visual rápido e encerramento
                _viewModel.SelectObject(obj);
                await Navigation.PopAsync();
                return;
            }
        }
    }

    private void OnPinchUpdated(object sender, PinchGestureUpdatedEventArgs e)
    {
        if (e.Status == GestureStatus.Started)
        {
            _startScale = ZoomableContainer.Scale;
            ZoomableContainer.AnchorX = e.ScaleOrigin.X;
            ZoomableContainer.AnchorY = e.ScaleOrigin.Y;
        }
        if (e.Status == GestureStatus.Running)
        {
            _currentScale += (e.Scale - 1) * _startScale;
            _currentScale = Math.Max(1, _currentScale); // Min Zoom x1
            _currentScale = Math.Min(_currentScale, 8); // Max Zoom x8

            ZoomableContainer.Scale = _currentScale;
        }
        if (e.Status == GestureStatus.Completed)
        {
            _xOffset = ZoomableContainer.TranslationX;
            _yOffset = ZoomableContainer.TranslationY;
        }
    }

    private void OnPanUpdated(object sender, PanUpdatedEventArgs e)
    {
        if (e.StatusType == GestureStatus.Running)
        {
            ZoomableContainer.TranslationX = _xOffset + e.TotalX;
            ZoomableContainer.TranslationY = _yOffset + e.TotalY;
        }
        else if (e.StatusType == GestureStatus.Completed)
        {
            _xOffset = ZoomableContainer.TranslationX;
            _yOffset = ZoomableContainer.TranslationY;
        }
    }
}
