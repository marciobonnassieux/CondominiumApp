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

    public double ImageWidth { get; private set; }
    public double ImageHeight { get; private set; }

    public System.Numerics.Matrix3x2 TransformationMatrix { get; set; } = System.Numerics.Matrix3x2.Identity;
    public Microsoft.Maui.Graphics.IImage? CapturedIImage { get; set; }

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
                // Opções para reduzir escala se imagem for muito grande na memória
                var options = new Android.Graphics.BitmapFactory.Options { InJustDecodeBounds = true };
                await Android.Graphics.BitmapFactory.DecodeByteArrayAsync(imageBytes, 0, imageBytes.Length, options);
                
                // Meta é limitar a imagem a ~1080p para o ML Kit (rápido e não arrebenta memória)
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

                // Usar JNI puro para invocar ML Kit sem depender de bindings C# específicos
                await ProcessWithJni(bitmap);

                bitmap.Recycle();
#endif
                await Task.CompletedTask;
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

#if ANDROID
    private async Task ProcessWithJni(Android.Graphics.Bitmap bitmap)
    {
        try
        {
            // Criar InputImage via JNI
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
                new Android.Runtime.JValue(0)
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

                // result é uma List<Barcode> — iterar
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
                            int l = Android.Runtime.JNIEnv.GetIntField(boxHandle, getLeft);
                            int t = Android.Runtime.JNIEnv.GetIntField(boxHandle, getTop);
                            int r = Android.Runtime.JNIEnv.GetIntField(boxHandle, getRight);
                            int b = Android.Runtime.JNIEnv.GetIntField(boxHandle, getBottom);
                            bounds = GetNormalizedBounds(l, t, r, b, ImageWidth, ImageHeight);
                        }
                        else
                        {
                            bounds = new Rect(0, 0, 1, 1);
                        }
                        Android.Runtime.JNIEnv.DeleteLocalRef(boxHandle);
                    }
                    else
                    {
                        bounds = new Rect(10, 10, ImageWidth - 20, ImageHeight - 20);
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

                // result é Text — obter blocks
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
                            int l = Android.Runtime.JNIEnv.GetIntField(boxHandle, getLeft);
                            int t = Android.Runtime.JNIEnv.GetIntField(boxHandle, getTop);
                            int r = Android.Runtime.JNIEnv.GetIntField(boxHandle, getRight);
                            int b = Android.Runtime.JNIEnv.GetIntField(boxHandle, getBottom);
                            bounds = GetNormalizedBounds(l, t, r, b, ImageWidth, ImageHeight);
                        }
                        else
                        {
                             bounds = new Rect(0, 0, 1, 1);
                        }
                        Android.Runtime.JNIEnv.DeleteLocalRef(boxHandle);
                    }
                    else
                    {
                        bounds = new Rect(10, 10, ImageWidth - 20, ImageHeight - 20);
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

        // Se o sensor físico da captura foi paisagem enquanto o celular estava em retrato, precisamos rotacionar +90º.
        if (imgW > imgH)
        {
            double rotX = imgH - (y + h);
            double rotY = x;
            double rotW = h;
            double rotH = w;
            return new Rect(rotY / imgW, (imgH - x - h) /*Wait rotX calculation below*/, rotW / imgH, rotH / imgW); 
        }

        return new Rect(x / imgW, y / imgH, w / imgW, h / imgH);
    }
#endif

    public void SelectObject(DetectedObject obj)
    {
        OnSelectionConfirmed?.Invoke(obj.Value);
    }
}
