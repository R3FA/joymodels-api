namespace JoyModels.Models.DataTransferObjects.ResponseTypes.ModelPicture;

public class ModelPictureResponse
{
    public Guid Uuid { get; set; }
    public Guid ModelUuid { get; set; }
    public string PictureLocation { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}