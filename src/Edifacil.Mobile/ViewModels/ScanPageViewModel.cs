using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Edifacil.Mobile.Models;
using System.Collections.ObjectModel;

namespace Edifacil.Mobile.ViewModels;

public partial class ScanPageViewModel : ObservableObject
{
    [ObservableProperty]
    private bool _isProcessing;

    [ObservableProperty]
    private ImageSource? _capturedImage;

    private byte[]? _lastImageBytes;
    private double _cropOffsetX = 0;
    private double _cropOffsetY = 0;
    private bool _isCroppedAnalysis = false;

    public ObservableCollection<DetectedObject> DetectedObjects { get; } = new();

    public Action<string>? OnSelectionConfirmed;

    public double ImageWidth { get; private set; }
    public double ImageHeight { get; private set; }

    public System.Numerics.Matrix3x2 TransformationMatrix { get; set; } = System.Numerics.Matrix3x2.Identity;
    public Microsoft.Maui.Graphics.IImage? CapturedIImage { get; set; }

    [RelayCommand]
    public async Task ProcessImageAsync(byte[] imageBytes)
    {
        IsProcessing = true;
        _lastImageBytes = imageBytes;
        _cropOffsetX = 0;
        _cropOffsetY = 0;
        _isCroppedAnalysis = false;
        DetectedObjects.Clear();

        try
        {
            await Task.Run(async () =>
            {
#if ANDROID
                // OpÃ§Ãµes para reduzir escala se imagem for muito grande na memÃ³ria
                var options = new Android.Graphics.BitmapFactory.Options { InJustDecodeBounds = true };
                await Android.Graphics.BitmapFactory.DecodeByteArrayAsync(imageBytes, 0, imageBytes.Length, options);
                
                // Meta Ã© limitar a imagem a ~1080p para o ML Kit (rÃ¡pido e nÃ£o arrebenta memÃ³ria)
                int inSampleSize = 1;
                if (options.OutHeight > 1080 || options.OutWidth > 1080)
                {
                    int halfHeight = options.OutHeight / 2;
                    int halfWidth = options.OutWidth / 2;
                    while ((halfHeight / inSampleSize) >= 1080 && (halfWidth / inSampleSize) >= 1080)
                    {
                        inSampleSize *= 2;
                    }
                }

                options.InJustDecodeBounds = false;
                options.InSampleSize = inSampleSize;
                using var bitmap = await Android.Graphics.BitmapFactory.DecodeByteArrayAsync(imageBytes, 0, imageBytes.Length, options);
                if (bitmap == null) return;

                ImageWidth = bitmap.Width;
                ImageHeight = bitmap.Height;

                // Usar JNI puro para invocar ML Kit sem depender de bindings C# especÃ­ficos
                await ProcessWithJni(bitmap);

                bitmap.Recycle();
#endif
                await Task.CompletedTask;
            });
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Erro no processamento de visÃ£o: {ex.Message}");
        }
        finally
        {
            IsProcessing = false;
        }
    }

#if ANDROID
    public async Task ReanalyzeZoomedAreaAsync(double viewW, double viewH)
    {
        if (_lastImageBytes == null || CapturedIImage == null) return;
        IsProcessing = true;
        try
        {
            double imgW = CapturedIImage.Width;
            double imgH = CapturedIImage.Height;
            
            if (System.Numerics.Matrix3x2.Invert(TransformationMatrix, out var inverted))
            {
                var tl = System.Numerics.Vector2.Transform(new System.Numerics.Vector2(0, 0), inverted);
                var br = System.Numerics.Vector2.Transform(new System.Numerics.Vector2((float)viewW, (float)viewH), inverted);

                double baseScale = Math.Min(viewW / imgW, viewH / imgH);
                double offsetX = (viewW - (imgW * baseScale)) / 2.0;
                double offsetY = (viewH - (imgH * baseScale)) / 2.0;

                double relLeft = (tl.X - offsetX) / (imgW * baseScale);
                double relTop = (tl.Y - offsetY) / (imgH * baseScale);
                double relRight = (br.X - offsetX) / (imgW * baseScale);
                double relBottom = (br.Y - offsetY) / (imgH * baseScale);

                relLeft = Math.Max(0, Math.Min(1, relLeft));
                relTop = Math.Max(0, Math.Min(1, relTop));
                relRight = Math.Max(0, Math.Min(1, relRight));
                relBottom = Math.Max(0, Math.Min(1, relBottom));

                if (relRight <= relLeft || relBottom <= relTop) return;

                await Task.Run(async () =>
                {
                    var options = new Android.Graphics.BitmapFactory.Options();
                    options.InSampleSize = 1; // ForÃ§ar mÃ¡xima minÃºcia em Lentes Crop!
                    
                    using var rawBitmap = await Android.Graphics.BitmapFactory.DecodeByteArrayAsync(_lastImageBytes, 0, _lastImageBytes.Length, options);
                    if (rawBitmap == null) return;

                    // Mapeia ProporÃ§Ã£o CCW da Tela Visual Portrait de Volta pro Buffer da Lente (RAW Landscape)
                    double rawRelTop = relLeft;
                    double rawRelBottom = relRight;
                    double rawRelLeft = 1.0 - relBottom;
                    double rawRelRight = 1.0 - relTop;

                    int cropX = (int)(rawRelLeft * rawBitmap.Width);
                    int cropY = (int)(rawRelTop * rawBitmap.Height);
                    int cropW = (int)((rawRelRight - rawRelLeft) * rawBitmap.Width);
                    int cropH = (int)((rawRelBottom - rawRelTop) * rawBitmap.Height);

                    cropX = Math.Max(0, Math.Min(rawBitmap.Width - 1, cropX));
                    cropY = Math.Max(0, Math.Min(rawBitmap.Height - 1, cropY));
                    if (cropX + cropW > rawBitmap.Width) cropW = rawBitmap.Width - cropX;
                    if (cropY + cropH > rawBitmap.Height) cropH = rawBitmap.Height - cropY;

                    if (cropW > 0 && cropH > 0)
                    {
                        var croppedBitmap = Android.Graphics.Bitmap.CreateBitmap(rawBitmap, cropX, cropY, cropW, cropH);
                        
                        _cropOffsetX = cropX;
                        _cropOffsetY = cropY;
                        _isCroppedAnalysis = true;

                        MainThread.BeginInvokeOnMainThread(() => DetectedObjects.Clear());
                        
                        await ProcessWithJni(croppedBitmap);
                        
                        croppedBitmap.Recycle();
                    }
                });
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Reanalyze Error: {ex.Message}");
        }
        finally
        {
            IsProcessing = false;
        }
    }

    private async Task ProcessWithJni(Android.Graphics.Bitmap bitmap)
    {
        try
        {
            int rotation = 0;

            if (ImageWidth > ImageHeight)
            {
                // CÃ¢mera do Android tirou foto Deitada (Landscape Sensor).
                // Informa o Google ML Kit para escanear a foto nativamente rodada +90 Graus, senÃ£o ele nÃ£o acha letras!
                rotation = 90;
            }

            // Criar InputImage via JNI passando a RotaÃ§Ã£o correta!
            var inputImageClass = Android.Runtime.JNIEnv.FindClass("com/google/mlkit/vision/common/InputImage");
            var fromBitmapMethod = Android.Runtime.JNIEnv.GetStaticMethodID(
                inputImageClass,
                "fromBitmap",
                "(Landroid/graphics/Bitmap;I)Lcom/google/mlkit/vision/common/InputImage;"
            );
            var inputImageHandle = Android.Runtime.JNIEnv.CallStaticObjectMethod(
                inputImageClass,
                fromBitmapMethod,
                new Android.Runtime.JValue(bitmap),
                new Android.Runtime.JValue(rotation)
            );

            if (inputImageHandle == IntPtr.Zero)
            {
                System.Diagnostics.Debug.WriteLine("Falha ao criar InputImage");
                return;
            }

            ProcessBarcodesJni(inputImageHandle);
            ProcessTextJni(inputImageHandle);

            Android.Runtime.JNIEnv.DeleteLocalRef(inputImageHandle);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"JNI ML Kit error: {ex.Message}");
        }
    }

    private void ProcessBarcodesJni(IntPtr inputImageHandle)
    {
        try
        {
            if (inputImageHandle == IntPtr.Zero) return;

                // BarcodeScanning.getClient()
                var scanningClass = Android.Runtime.JNIEnv.FindClass("com/google/mlkit/vision/barcode/BarcodeScanning");
                if (scanningClass == IntPtr.Zero) return;

                var getClientMethod = Android.Runtime.JNIEnv.GetStaticMethodID(
                    scanningClass, "getClient",
                    "()Lcom/google/mlkit/vision/barcode/BarcodeScanner;"
                );
                if (getClientMethod == IntPtr.Zero) return;

                var scannerHandle = Android.Runtime.JNIEnv.CallStaticObjectMethod(scanningClass, getClientMethod);
                if (scannerHandle == IntPtr.Zero) return;

                // scanner.process(inputImage)
                var scannerClass = Android.Runtime.JNIEnv.GetObjectClass(scannerHandle);
                var processMethod = Android.Runtime.JNIEnv.GetMethodID(
                    scannerClass, "process",
                    "(Lcom/google/mlkit/vision/common/InputImage;)Lcom/google/android/gms/tasks/Task;"
                );
                if (processMethod == IntPtr.Zero) return;

                var taskHandle = Android.Runtime.JNIEnv.CallObjectMethod(scannerHandle, processMethod,
                    new Android.Runtime.JValue(inputImageHandle));
                if (taskHandle == IntPtr.Zero) return;

                // Aguardar a task via Tasks.await()
                var tasksClass = Android.Runtime.JNIEnv.FindClass("com/google/android/gms/tasks/Tasks");
                if (tasksClass == IntPtr.Zero) return;

                var awaitMethod = Android.Runtime.JNIEnv.GetStaticMethodID(
                    tasksClass, "await",
                    "(Lcom/google/android/gms/tasks/Task;)Ljava/lang/Object;"
                );
                if (awaitMethod == IntPtr.Zero) return;

                var resultHandle = Android.Runtime.JNIEnv.CallStaticObjectMethod(tasksClass, awaitMethod,
                    new Android.Runtime.JValue(taskHandle));

                if (resultHandle == IntPtr.Zero) return;

                // result Ã© uma List<Barcode> â€” iterar
                var listClass = Android.Runtime.JNIEnv.FindClass("java/util/List");
                var sizeMethod = Android.Runtime.JNIEnv.GetMethodID(listClass, "size", "()I");
                var getMethod = Android.Runtime.JNIEnv.GetMethodID(listClass, "get", "(I)Ljava/lang/Object;");

                if (sizeMethod == IntPtr.Zero || getMethod == IntPtr.Zero) return;

                int count = Android.Runtime.JNIEnv.CallIntMethod(resultHandle, sizeMethod);
                var barcodeClass = Android.Runtime.JNIEnv.FindClass("com/google/mlkit/vision/barcode/common/Barcode");
                if (barcodeClass == IntPtr.Zero) return;

                var getRawValueMethod = Android.Runtime.JNIEnv.GetMethodID(barcodeClass, "getRawValue", "()Ljava/lang/String;");
                var getBoundingBoxMethod = Android.Runtime.JNIEnv.GetMethodID(barcodeClass, "getBoundingBox", "()Landroid/graphics/Rect;");

                for (int i = 0; i < count; i++)
                {
                    var itemHandle = Android.Runtime.JNIEnv.CallObjectMethod(resultHandle, getMethod, new Android.Runtime.JValue(i));
                    if (itemHandle == IntPtr.Zero) continue;

                    var rawHandle = Android.Runtime.JNIEnv.CallObjectMethod(itemHandle, getRawValueMethod);
                    var rawValue = rawHandle != IntPtr.Zero ? Android.Runtime.JNIEnv.GetString(rawHandle, Android.Runtime.JniHandleOwnership.TransferLocalRef) : "";

                    var boxHandle = Android.Runtime.JNIEnv.CallObjectMethod(itemHandle, getBoundingBoxMethod);
                    Rect bounds;
                    if (boxHandle != IntPtr.Zero)
                    {
                        var rectClass = Android.Runtime.JNIEnv.GetObjectClass(boxHandle);
                        var getLeft = Android.Runtime.JNIEnv.GetFieldID(rectClass, "left", "I");
                        var getTop = Android.Runtime.JNIEnv.GetFieldID(rectClass, "top", "I");
                        var getRight = Android.Runtime.JNIEnv.GetFieldID(rectClass, "right", "I");
                        var getBottom = Android.Runtime.JNIEnv.GetFieldID(rectClass, "bottom", "I");
                        
                        if (getLeft != IntPtr.Zero && getBottom != IntPtr.Zero)
                        {
                            int l = Android.Runtime.JNIEnv.GetIntField(boxHandle, getLeft) + (int)_cropOffsetX;
                            int t = Android.Runtime.JNIEnv.GetIntField(boxHandle, getTop) + (int)_cropOffsetY;
                            int r = Android.Runtime.JNIEnv.GetIntField(boxHandle, getRight) + (int)_cropOffsetX;
                            int b = Android.Runtime.JNIEnv.GetIntField(boxHandle, getBottom) + (int)_cropOffsetY;
                            bounds = GetNormalizedBounds(l, t, r, b, ImageWidth, ImageHeight);
                        }
                        else
                        {
                            Android.Runtime.JNIEnv.DeleteLocalRef(boxHandle);
                            Android.Runtime.JNIEnv.DeleteLocalRef(itemHandle);
                            continue;
                        }
                        Android.Runtime.JNIEnv.DeleteLocalRef(boxHandle);
                    }
                    else
                    {
                        Android.Runtime.JNIEnv.DeleteLocalRef(itemHandle);
                        continue; // Sem caixinha grÃ¡fica, item veta
                    }

                    if (!string.IsNullOrEmpty(rawValue))
                    {
                        var capturedValue = rawValue;
                        var capturedBounds = bounds;
                        MainThread.BeginInvokeOnMainThread(() =>
                        {
                            DetectedObjects.Add(new DetectedObject
                            {
                                Value = capturedValue,
                                Type = DetectionType.Barcode,
                                RelativeBounds = capturedBounds
                            });
                        });
                    }

                    Android.Runtime.JNIEnv.DeleteLocalRef(itemHandle);
                }

                Android.Runtime.JNIEnv.DeleteLocalRef(resultHandle);
                Android.Runtime.JNIEnv.DeleteLocalRef(taskHandle);
                Android.Runtime.JNIEnv.DeleteLocalRef(scannerHandle);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Barcode JNI error: {ex.Message}");
            }
    }

    private void ProcessTextJni(IntPtr inputImageHandle)
    {
        try
        {
            if (inputImageHandle == IntPtr.Zero) return;

                // TextRecognition.getClient(options)
                var latinOptionClass = Android.Runtime.JNIEnv.FindClass("com/google/mlkit/vision/text/latin/TextRecognizerOptions");
                if (latinOptionClass == IntPtr.Zero) return;
                
                var defaultOptionsField = Android.Runtime.JNIEnv.GetStaticFieldID(
                    latinOptionClass, "DEFAULT_OPTIONS",
                    "Lcom/google/mlkit/vision/text/latin/TextRecognizerOptions;"
                );
                if (defaultOptionsField == IntPtr.Zero) return;
                
                var optionsHandle = Android.Runtime.JNIEnv.GetStaticObjectField(latinOptionClass, defaultOptionsField);
                if (optionsHandle == IntPtr.Zero) return;

                var textRecClass = Android.Runtime.JNIEnv.FindClass("com/google/mlkit/vision/text/TextRecognition");
                if (textRecClass == IntPtr.Zero) return;
                
                var getClientMethod = Android.Runtime.JNIEnv.GetStaticMethodID(
                    textRecClass, "getClient",
                    "(Lcom/google/mlkit/vision/text/TextRecognizerOptionsInterface;)Lcom/google/mlkit/vision/text/TextRecognizer;"
                );
                if (getClientMethod == IntPtr.Zero) return;
                
                var recognizerHandle = Android.Runtime.JNIEnv.CallStaticObjectMethod(textRecClass, getClientMethod,
                    new Android.Runtime.JValue(optionsHandle));
                if (recognizerHandle == IntPtr.Zero) return;

                var recognizerClass = Android.Runtime.JNIEnv.GetObjectClass(recognizerHandle);
                var processMethod = Android.Runtime.JNIEnv.GetMethodID(
                    recognizerClass, "process",
                    "(Lcom/google/mlkit/vision/common/InputImage;)Lcom/google/android/gms/tasks/Task;"
                );
                if (processMethod == IntPtr.Zero) return;
                
                var taskHandle = Android.Runtime.JNIEnv.CallObjectMethod(recognizerHandle, processMethod,
                    new Android.Runtime.JValue(inputImageHandle));
                if (taskHandle == IntPtr.Zero) return;

                var tasksClass = Android.Runtime.JNIEnv.FindClass("com/google/android/gms/tasks/Tasks");
                var awaitMethod = Android.Runtime.JNIEnv.GetStaticMethodID(
                    tasksClass, "await",
                    "(Lcom/google/android/gms/tasks/Task;)Ljava/lang/Object;"
                );
                var resultHandle = Android.Runtime.JNIEnv.CallStaticObjectMethod(tasksClass, awaitMethod,
                    new Android.Runtime.JValue(taskHandle));

                if (resultHandle == IntPtr.Zero) return;

                // result Ã© Text â€” obter blocks
                var textClass = Android.Runtime.JNIEnv.GetObjectClass(resultHandle);
                var getBlocksMethod = Android.Runtime.JNIEnv.GetMethodID(textClass, "getTextBlocks", "()Ljava/util/List;");
                if (getBlocksMethod == IntPtr.Zero) return;
                
                var blocksHandle = Android.Runtime.JNIEnv.CallObjectMethod(resultHandle, getBlocksMethod);
                if (blocksHandle == IntPtr.Zero) return;

                var listClass = Android.Runtime.JNIEnv.FindClass("java/util/List");
                var sizeMethod = Android.Runtime.JNIEnv.GetMethodID(listClass, "size", "()I");
                var getMethod = Android.Runtime.JNIEnv.GetMethodID(listClass, "get", "(I)Ljava/lang/Object;");
                int count = Android.Runtime.JNIEnv.CallIntMethod(blocksHandle, sizeMethod);

                var blockClass = Android.Runtime.JNIEnv.FindClass("com/google/mlkit/vision/text/Text$TextBlock");
                var getTextMethod = Android.Runtime.JNIEnv.GetMethodID(blockClass, "getText", "()Ljava/lang/String;");
                var getBboxMethod = Android.Runtime.JNIEnv.GetMethodID(blockClass, "getBoundingBox", "()Landroid/graphics/Rect;");

                for (int i = 0; i < count; i++)
                {
                    var blockHandle = Android.Runtime.JNIEnv.CallObjectMethod(blocksHandle, getMethod, new Android.Runtime.JValue(i));
                    if (blockHandle == IntPtr.Zero) continue;

                    var textHandle = Android.Runtime.JNIEnv.CallObjectMethod(blockHandle, getTextMethod);
                    var text = textHandle != IntPtr.Zero ? Android.Runtime.JNIEnv.GetString(textHandle, Android.Runtime.JniHandleOwnership.TransferLocalRef) : "";

                    var boxHandle = Android.Runtime.JNIEnv.CallObjectMethod(blockHandle, getBboxMethod);
                    Rect bounds;
                    if (boxHandle != IntPtr.Zero)
                    {
                        var rectClass = Android.Runtime.JNIEnv.GetObjectClass(boxHandle);
                        var getLeft = Android.Runtime.JNIEnv.GetFieldID(rectClass, "left", "I");
                        var getTop = Android.Runtime.JNIEnv.GetFieldID(rectClass, "top", "I");
                        var getRight = Android.Runtime.JNIEnv.GetFieldID(rectClass, "right", "I");
                        var getBottom = Android.Runtime.JNIEnv.GetFieldID(rectClass, "bottom", "I");
                        
                        if (getLeft != IntPtr.Zero && getBottom != IntPtr.Zero)
                        {
                            int l = Android.Runtime.JNIEnv.GetIntField(boxHandle, getLeft) + (int)_cropOffsetX;
                            int t = Android.Runtime.JNIEnv.GetIntField(boxHandle, getTop) + (int)_cropOffsetY;
                            int r = Android.Runtime.JNIEnv.GetIntField(boxHandle, getRight) + (int)_cropOffsetX;
                            int b = Android.Runtime.JNIEnv.GetIntField(boxHandle, getBottom) + (int)_cropOffsetY;
                            bounds = GetNormalizedBounds(l, t, r, b, ImageWidth, ImageHeight);
                        }
                        else
                        {
                            Android.Runtime.JNIEnv.DeleteLocalRef(boxHandle);
                            Android.Runtime.JNIEnv.DeleteLocalRef(blockHandle);
                            continue;
                        }
                        Android.Runtime.JNIEnv.DeleteLocalRef(boxHandle);
                    }
                    else
                    {
                        Android.Runtime.JNIEnv.DeleteLocalRef(blockHandle);
                        continue;
                    }

                    if (!string.IsNullOrWhiteSpace(text))
                    {
                        var capturedText = text;
                        var capturedBounds = bounds;
                        MainThread.BeginInvokeOnMainThread(() =>
                        {
                            DetectedObjects.Add(new DetectedObject
                            {
                                Value = capturedText,
                                Type = DetectionType.Text,
                                RelativeBounds = capturedBounds
                            });
                        });
                    }

                    Android.Runtime.JNIEnv.DeleteLocalRef(blockHandle);
                }

                Android.Runtime.JNIEnv.DeleteLocalRef(blocksHandle);
                Android.Runtime.JNIEnv.DeleteLocalRef(resultHandle);
                Android.Runtime.JNIEnv.DeleteLocalRef(taskHandle);
                Android.Runtime.JNIEnv.DeleteLocalRef(recognizerHandle);
                Android.Runtime.JNIEnv.DeleteLocalRef(optionsHandle);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"OCR JNI error: {ex.Message}");
            }
    }

    private Rect GetNormalizedBounds(int left, int top, int right, int bottom, double imgW, double imgH)
    {
        double x = left, y = top, w = right - left, h = bottom - top;
        // O algoritmo C++ do Google processa na ResoluÃ§Ã£o do Sensor Bruta Larga (e.j. 4000x3000 Horizontal).
        // Pelo relato do usuÃ¡rio, o modelo do celular opera com o sensor montado de forma que o eixo X estava invertido em 180Âº.
        // A matriz correta para casar o buffer de dados horizontais deste Hardware com a foto vertical +EXIF da Tela Ã© +90 CCW:
        if (imgW > imgH)
        {
            double newLeft = top;
            double newTop = imgW - right;
            double newWidth = bottom - top;
            double newHeight = right - left;
            
            // Retorna as porcentagens relativas normalizadas usando as extremidades da Tela Portrait (Width=ImgH, Height=ImgW)
            return new Rect(newLeft / imgH, newTop / imgW, newWidth / imgH, newHeight / imgW);
        }

        return new Rect(x / imgW, y / imgH, w / imgW, h / imgH);
    }
#endif

    public void SelectObject(DetectedObject obj)
    {
        OnSelectionConfirmed?.Invoke(obj.Value);
    }
}

