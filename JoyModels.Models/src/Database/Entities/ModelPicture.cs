using System;
using System.Collections.Generic;

namespace JoyModels.Models.src.Database.Entities;

public partial class ModelPicture
{
    public Guid Uuid { get; set; }

    public Guid ModelUuid { get; set; }

    public string PictureLocation { get; set; } = null!;

    public int PictureWidth { get; set; }

    public int PictureHeight { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Model ModelUu { get; set; } = null!;
}
