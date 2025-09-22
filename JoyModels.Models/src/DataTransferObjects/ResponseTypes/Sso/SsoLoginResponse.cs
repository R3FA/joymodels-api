namespace JoyModels.Models.DataTransferObjects.ResponseTypes.Sso;

public class SsoLoginResponse
{
    public string AccessToken { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
}