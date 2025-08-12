using System;
using System.Collections.Generic;

namespace JoyModels.Models.src.Database.Entities;

public partial class UserDevice
{
    public Guid Uuid { get; set; }

    public string DeviceType { get; set; } = null!;

    public virtual ICollection<UserToken> UserTokens { get; set; } = new List<UserToken>();
}
