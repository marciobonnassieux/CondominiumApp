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
        if (ViewModel == null || ViewModel.CapturedIImage == null || ViewModel.ImageWidth <= 0 || ViewModel.ImageHeight <= 0)
            return;

        canvas.SaveState();
        canvas.ConcatenateTransform(ViewModel.TransformationMatrix);

        // Cálculo de Matriz AspectFit
        double imgW = ViewModel.CapturedIImage.Width;
        double imgH = ViewModel.CapturedIImage.Height;
        double viewW = viewRect.Width;
        double viewH = viewRect.Height;

        float scale = (float)Math.Min(viewW / imgW, viewH / imgH);
        float offsetX = (float)((viewW - (imgW * scale)) / 2.0);
        float offsetY = (float)((viewH - (imgH * scale)) / 2.0);

        canvas.DrawImage(ViewModel.CapturedIImage, offsetX, offsetY, (float)imgW * scale, (float)imgH * scale);

        float inverseScaleZoom = 1f;
        if(ViewModel.TransformationMatrix.M11 > 0)
        {
            inverseScaleZoom = 1f / ViewModel.TransformationMatrix.M11;
        }

        foreach (var obj in ViewModel.DetectedObjects)
        {
            canvas.StrokeColor = obj.GetDisplayColor();
            canvas.StrokeSize = 4 * inverseScaleZoom;
            canvas.FillColor = obj.GetDisplayColor().WithAlpha(0.2f);

            float x = offsetX + (float)(obj.RelativeBounds.X * imgW * scale);
            float y = offsetY + (float)(obj.RelativeBounds.Y * imgH * scale);
            float w = (float)(obj.RelativeBounds.Width * imgW * scale);
            float h = (float)(obj.RelativeBounds.Height * imgH * scale);

            canvas.DrawRoundedRectangle(x, y, w, h, 8 * inverseScaleZoom);
            canvas.FillRoundedRectangle(x, y, w, h, 8 * inverseScaleZoom);
        }

        canvas.RestoreState();
    }
}

public partial class ScanPage : ContentPage
{
    public Action<string>? OnCodeDetected;
    private ScanPageViewModel _viewModel;
    private PhotoScannerDrawable _drawable;

    private double _currentScale = 1;
    private double _xOffset = 0;
    private double _yOffset = 0;

    private bool _isMoveMode = false;
    private double _panStartX = 0;
    private double _panStartY = 0;

    public ScanPage()
    {
        InitializeComponent();
        _viewModel = new ScanPageViewModel();
        BindingContext = _viewModel;
        
        _drawable = new PhotoScannerDrawable { ViewModel = _viewModel };
        ArOverlay.Drawable = _drawable;

        _viewModel.DetectedObjects.CollectionChanged += (s, e) => 
        {
            ArOverlay.Invalidate();
        };

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
                
                ms.Position = 0;
                _viewModel.CapturedIImage = Microsoft.Maui.Graphics.Platform.PlatformImage.FromStream(ms);
            }
        }

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
        _viewModel.CapturedIImage = null;
        
        // Reset do Zoom, Pan e Modos
        _currentScale = 1;
        _xOffset = 0;
        _yOffset = 0;
        _isMoveMode = false;
        ModeBtn.TextColor = Colors.White;
        ModeBtn.BackgroundColor = Colors.Transparent;
        _viewModel.TransformationMatrix = System.Numerics.Matrix3x2.Identity;

        ReviewGrid.IsVisible = false;
        CameraGrid.IsVisible = true;
        _viewModel.DetectedObjects.Clear();
        ArOverlay.Invalidate();
    }

    private async void OnOverlayTapped(object sender, TappedEventArgs e)
    {
        if (_isMoveMode) return;

        var touchPos = e.GetPosition(ArOverlay);
        if (touchPos == null || !_viewModel.DetectedObjects.Any()) return;

        double touchX = touchPos.Value.X;
        double touchY = touchPos.Value.Y;

        // Hit Testing Matemático usando Inversive Matrix 
        if (System.Numerics.Matrix3x2.Invert(_viewModel.TransformationMatrix, out var inverted))
        {
            // O toque é na View. Transforma para o espaço de mundo virtual:
            var virtualTouch = System.Numerics.Vector2.Transform(new System.Numerics.Vector2((float)touchX, (float)touchY), inverted);
            
            double imgW = _viewModel.CapturedIImage.Width;
            double imgH = _viewModel.CapturedIImage.Height;
            double viewW = ArOverlay.Width;
            double viewH = ArOverlay.Height;

            double scale = Math.Min(viewW / imgW, viewH / imgH);
            double offsetX = (viewW - (imgW * scale)) / 2.0;
            double offsetY = (viewH - (imgH * scale)) / 2.0;

            foreach (var obj in _viewModel.DetectedObjects)
            {
                double x = (obj.RelativeBounds.X * imgW * scale) + offsetX;
                double y = (obj.RelativeBounds.Y * imgH * scale) + offsetY;
                double w = (obj.RelativeBounds.Width * imgW * scale);
                double h = (obj.RelativeBounds.Height * imgH * scale);

                if (virtualTouch.X >= x && virtualTouch.X <= x + w && virtualTouch.Y >= y && virtualTouch.Y <= y + h)
                {
                    _viewModel.SelectObject(obj);
                    await Navigation.PopAsync();
                    return;
                }
            }
        }
    }

    private void OnZoomInClicked(object sender, EventArgs e) => ApplyZoom(0.5);

    private void OnZoomOutClicked(object sender, EventArgs e) => ApplyZoom(-0.5);

    private void ApplyZoom(double delta)
    {
        double oldScale = _currentScale;
        _currentScale += delta;
        _currentScale = Math.Max(1, _currentScale); 
        _currentScale = Math.Min(_currentScale, 8); 

        if (_currentScale == oldScale) return;

        double scaleFactor = _currentScale / oldScale;

        // Focado no meio visual exato da tela. O Canvas nativo trabalha em Pixels Físicos na Matriz (ignora DPs).
        // Devemos converter o 'Width' e 'Height' (Que vem em DP) para HW Pixels usando a Densidade de Hardware:
        double density = DeviceDisplay.MainDisplayInfo.Density;
        double originX = (ArOverlay.Width * density) / 2.0; 
        double originY = (ArOverlay.Height * density) / 2.0;  

        _xOffset = originX - (originX - _xOffset) * scaleFactor;
        _yOffset = originY - (originY - _yOffset) * scaleFactor;

        UpdateMatrix();
    }

    private void OnModeToggleClicked(object sender, EventArgs e)
    {
        _isMoveMode = !_isMoveMode;
        if (_isMoveMode)
        {
            ModeBtn.TextColor = Color.FromArgb("#F1C40F"); // Highlight (Ativado)
            ModeBtn.BackgroundColor = Color.FromArgb("#33F1C40F"); // Fundo sutil
        }
        else
        {
            ModeBtn.TextColor = Colors.White; // Normal (Desativado)
            ModeBtn.BackgroundColor = Colors.Transparent;
        }
    }

    private void OnPanUpdated(object sender, PanUpdatedEventArgs e)
    {
        if (!_isMoveMode) return; 

        if (e.StatusType == GestureStatus.Started)
        {
            _panStartX = _xOffset;
            _panStartY = _yOffset;
        }
        else if (e.StatusType == GestureStatus.Running)
        {
            // O sensor de toque de Android envia os deltas de forma invertida e transposta em relação ao GraphicsView.
            // Para eliminar a "Velocidade Parallax Lenta", multplicamos pelos Pixels Fisícos por via de Densidade:
            double density = DeviceDisplay.MainDisplayInfo.Density;
            
            double fixedTotalX = (e.TotalY) * density;
            double fixedTotalY = (e.TotalX) * density; 

            _xOffset = _panStartX + fixedTotalX;
            _yOffset = _panStartY + fixedTotalY;
            UpdateMatrix();
        }
    }

    private void UpdateMatrix()
    {
        _viewModel.TransformationMatrix = 
            System.Numerics.Matrix3x2.CreateScale((float)_currentScale) * 
            System.Numerics.Matrix3x2.CreateTranslation((float)_xOffset, (float)_yOffset);
        
        ArOverlay.Invalidate();
    }
}
