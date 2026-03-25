namespace Edifacil.Mobile.Models;

public enum DetectionType
{
    Text,
    QrCode,
    Barcode
}

public class DetectedObject
{
    public string Value { get; set; } = string.Empty;
    public Rect RelativeBounds { get; set; } // Coordenadas normalizadas de 0.0 a 1.0
    public DetectionType Type { get; set; }

    public Color GetDisplayColor() => Type switch
    {
        DetectionType.Text => Colors.RoyalBlue,
        DetectionType.QrCode => Colors.LawnGreen,
        DetectionType.Barcode => Colors.Orange,
        _ => Colors.Gray
    };
}
