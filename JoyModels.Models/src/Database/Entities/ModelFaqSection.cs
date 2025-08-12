using System;
using System.Collections.Generic;

namespace JoyModels.Models.src.Database.Entities;

public partial class ModelFaqSection
{
    public Guid Uuid { get; set; }

    public Guid UserOriginUuid { get; set; }

    public Guid? UserTargetUuid { get; set; }

    public Guid ModelUuid { get; set; }

    public Guid MessageTypeUuid { get; set; }

    public string MessageText { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual MessageType MessageTypeUu { get; set; } = null!;

    public virtual Model ModelUu { get; set; } = null!;

    public virtual User UserOriginUu { get; set; } = null!;

    public virtual User? UserTargetUu { get; set; }
}
