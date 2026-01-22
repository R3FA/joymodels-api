using JoyModels.Models.DataTransferObjects.ResponseTypes.Models;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Order;

namespace JoyModels.Models.DataTransferObjects.ResponseTypes.Library;

public class LibraryResponse
{
    public Guid Uuid { get; set; }
    public ModelResponse Model { get; set; } = null!;
    public OrderResponse Order { get; set; } = null!;
    public DateTime AcquiredAt { get; set; }
}