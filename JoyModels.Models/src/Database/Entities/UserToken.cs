using System;
using System.Collections.Generic;

namespace JoyModels.Models.src.Database.Entities;

public partial class UserToken
{
    public Guid Uuid { get; set; }

    public Guid UserUuid { get; set; }

    public Guid UserDeviceUuid { get; set; }

    public string RefreshToken { get; set; } = null!;

    public DateTime TokenCreatedAt { get; set; }

    public DateTime TokenExpirationDate { get; set; }

    public virtual UserDevice UserDeviceUu { get; set; } = null!;

    public virtual User UserUu { get; set; } = null!;
}
