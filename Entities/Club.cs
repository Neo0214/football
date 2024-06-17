using System;
using System.Collections.Generic;

namespace football.Entities;

public partial class Club
{
    public int ClubId { get; set; }

    public string? ClubName { get; set; }

    public int? Score { get; set; }

    public virtual ICollection<Transfer> TransferFromNavigations { get; set; } = new List<Transfer>();

    public virtual ICollection<Transfer> TransferToNavigations { get; set; } = new List<Transfer>();

    public virtual ICollection<Player> Players { get; set; } = new List<Player>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
