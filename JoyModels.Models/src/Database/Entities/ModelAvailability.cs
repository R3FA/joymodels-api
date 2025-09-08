using System;
using System.Collections.Generic;

namespace JoyModels.Models.src.Database.Entities;

public partial class ModelAvailability
{
    public Guid Uuid { get; set; }

    public string AvailabilityName { get; set; } = null!;

    public virtual ICollection<Model> Models { get; set; } = new List<Model>();
}
