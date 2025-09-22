namespace JoyModels.Models.DataTransferObjects.RequestTypes.Sso;

public class SsoLoginRequest
{
    public string Nickname { get; set; } = null!;
    public string Password { get; set; } = null!;
}