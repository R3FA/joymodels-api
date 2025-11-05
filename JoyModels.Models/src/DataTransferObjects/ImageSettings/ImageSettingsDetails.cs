namespace JoyModels.Models.DataTransferObjects.ImageSettings;

public class ImageSettingsDetails
{
    public ImageSettingsResolutionDetails ImageSettingsResolutionDetails { get; set; } = null!;
    public int AllowedSize { get; set; }
    public string SavePath { get; set; } = string.Empty;
}