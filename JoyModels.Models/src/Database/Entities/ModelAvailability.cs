namespace JoyModels.Models.Database.Entities;

public partial class ModelAvailability
{
    public Guid Uuid { get; set; }

    public string AvailabilityName { get; set; } = null!;

    public virtual ICollection<Model> Models { get; set; } = new List<Model>();
}