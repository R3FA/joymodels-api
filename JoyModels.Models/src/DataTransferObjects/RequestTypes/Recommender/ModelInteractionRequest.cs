using Microsoft.ML.Data;

namespace JoyModels.Models.DataTransferObjects.RequestTypes.Recommender;

public class ModelInteractionRequest
{
    [KeyType(count: 100000)] public uint UserId { get; set; }

    [KeyType(count: 100000)] public uint ModelId { get; set; }

    public float Score { get; set; }
}