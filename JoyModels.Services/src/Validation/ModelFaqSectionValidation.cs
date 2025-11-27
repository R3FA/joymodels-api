using JoyModels.Models.Database;
using JoyModels.Models.DataTransferObjects.RequestTypes.ModelFaqSection;
using Microsoft.EntityFrameworkCore;

namespace JoyModels.Services.Validation;

public static class ModelFaqSectionValidation
{
    private static void ValidateMessageText(string messageText)
    {
        if (!string.IsNullOrWhiteSpace(messageText))
            RegularExpressionValidation.ValidateText(messageText);
    }

    public static void ValidateModelFaqSectionCreateArguments(this ModelFaqSectionCreateRequest request)
        => ValidateMessageText(request.MessageText);

    public static async Task ValidateModelFaqSectionCreateAnswerArguments(
        this ModelFaqSectionCreateAnswerRequest request,
        JoyModelsDbContext context)
    {
        ValidateMessageText(request.MessageText);

        var targetMessage = await context.ModelFaqSections
            .AsNoTracking()
            .Select(x => new { x.Uuid, x.ParentMessageUuid })
            .FirstOrDefaultAsync(x => x.Uuid == request.ParentMessageUuid);

        if (targetMessage == null)
            throw new KeyNotFoundException("Question that you want to answer to does not exist.");

        if (targetMessage.ParentMessageUuid != null)
            throw new ArgumentException("You cannot answer to an answer of FAQ section.");
    }

    public static void ValidateModelFaqSectionPatchArguments(this ModelFaqSectionPatchRequest request)
        => ValidateMessageText(request.MessageText);
}