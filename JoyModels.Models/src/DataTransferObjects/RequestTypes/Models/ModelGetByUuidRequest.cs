using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace JoyModels.Models.DataTransferObjects.RequestTypes.Models;

public class ModelGetByUuidRequest
{
    [Required] public Guid ModelUuid { get; set; }
    [DefaultValue(false)] public bool ArePrivateModelsSearched { get; set; }
}