namespace JoyModels.Models.DataTransferObjects.ResponseTypes.Core;

public class PictureResponse
{
    public byte[] FileBytes { get; set; } = null!;
    public string ContentType { get; set; } = string.Empty;
}