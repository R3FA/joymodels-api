using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace JoyModels.API.Setups.DatabaseSeed.Helpers;

public static class ColorUtilities
{
    public static Color DarkenColor(Color color, float amount)
    {
        var pixel = color.ToPixel<Rgba32>();
        return Color.FromRgba(
            (byte)(pixel.R * (1 - amount)),
            (byte)(pixel.G * (1 - amount)),
            (byte)(pixel.B * (1 - amount)),
            pixel.A);
    }

    public static Color LightenColor(Color color, float amount)
    {
        var pixel = color.ToPixel<Rgba32>();
        return Color.FromRgba(
            (byte)(pixel.R + (255 - pixel.R) * amount),
            (byte)(pixel.G + (255 - pixel.G) * amount),
            (byte)(pixel.B + (255 - pixel.B) * amount),
            pixel.A);
    }
}