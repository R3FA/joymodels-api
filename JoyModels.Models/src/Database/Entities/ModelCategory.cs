using System;
using System.Collections.Generic;

namespace JoyModels.Models.src.Database.Entities;

public partial class ModelCategory
{
    public Guid Uuid { get; set; }

    public Guid ModelUuid { get; set; }

    public Guid CategoryUuid { get; set; }

    public virtual Category CategoryUu { get; set; } = null!;

    public virtual Model ModelUu { get; set; } = null!;
}
