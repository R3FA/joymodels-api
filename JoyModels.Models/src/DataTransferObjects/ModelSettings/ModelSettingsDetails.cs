namespace JoyModels.Models.DataTransferObjects.ModelSettings;

public class ModelSettingsDetails
{
    public int AllowedSize { get; set; }
    public List<string> AllowedFormats { get; set; } = null!;
    public string SavePath { get; set; } = string.Empty;
}