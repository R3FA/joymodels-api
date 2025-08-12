using System;
using System.Collections.Generic;

namespace JoyModels.Models.src.Database.Entities;

public partial class Category
{
    public Guid Uuid { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<ModelCategory> ModelCategories { get; set; } = new List<ModelCategory>();
}
