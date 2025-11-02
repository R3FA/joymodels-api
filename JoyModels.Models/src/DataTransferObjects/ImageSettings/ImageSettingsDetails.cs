namespace JoyModels.Models.DataTransferObjects.ImageSettings;

public class ImageSettingsDetails
{
    public int AllowedSize { get; set; }
    public string[] AllowedExtensions { get; set; } = [];
    public string SavePath { get; set; } = string.Empty;
}