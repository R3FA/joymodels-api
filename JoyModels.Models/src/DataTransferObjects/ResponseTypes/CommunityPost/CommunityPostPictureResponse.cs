namespace JoyModels.Models.DataTransferObjects.ResponseTypes.CommunityPost;

public class CommunityPostPictureResponse
{
    public Guid Uuid { get; set; }
    public string PictureLocation { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}