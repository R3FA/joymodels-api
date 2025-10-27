using JoyModels.Models.DataTransferObjects.RequestTypes.Email;

namespace JoyModels.Communications.Services;

public interface IEmailService
{
    Task SendEmailAsync(EmailSendRequest request);
}