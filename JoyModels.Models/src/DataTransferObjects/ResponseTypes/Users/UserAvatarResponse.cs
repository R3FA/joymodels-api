namespace JoyModels.Models.DataTransferObjects.ResponseTypes.Users;

public class UserAvatarResponse
{
    public byte[] FileBytes { get; set; } = null!;
    public string ContentType { get; set; } = string.Empty;
}