using JoyModels.Models.Database;
using JoyModels.Models.DataTransferObjects.RequestTypes.CommunityPostQuestionSection;
using Microsoft.EntityFrameworkCore;

namespace JoyModels.Services.Validation;

public static class CommunityPostQuestionSectionValidation
{
    private static void ValidateMessageText(string messageText)
    {
        if (!string.IsNullOrWhiteSpace(messageText))
            RegularExpressionValidation.ValidateText(messageText);
    }

    public static void ValidateCommunityPostQuestionSectionCreateArguments(
        this CommunityPostQuestionSectionCreateRequest request)
        => ValidateMessageText(request.MessageText);

    public static async Task ValidateCommunityPostSectionCreateAnswerArguments(
        this CommunityPostQuestionSectionCreateAnswerRequest request,
        JoyModelsDbContext context)
    {
        ValidateMessageText(request.MessageText);

        var targetMessage = await context.CommunityPostQuestionSections
            .AsNoTracking()
            .Select(x => new { x.Uuid, x.ParentMessageUuid })
            .FirstOrDefaultAsync(x => x.Uuid == request.ParentMessageUuid);

        if (targetMessage == null)
            throw new KeyNotFoundException("Question that you want to answer to does not exist.");

        if (targetMessage.ParentMessageUuid != null)
            throw new ArgumentException("You cannot answer to an answer in community posts.");
    }

    public static void ValidateCommunityPostQuestionSectionPatchArguments(
        this CommunityPostQuestionSectionPatchRequest request)
        => ValidateMessageText(request.MessageText);
}