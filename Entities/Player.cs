using System;
using System.Collections.Generic;

namespace football.Entities;

public partial class Player
{
    public int PlayerId { get; set; }

    public string? Name { get; set; }

    public string? Position { get; set; }

    public int? Value { get; set; }

    public DateOnly? ExpireDate { get; set; }

    public int? Score { get; set; }

    public int? Shot { get; set; }

    public int? Pass { get; set; }

    public int? Dribble { get; set; }

    public int? Defence { get; set; }

    public int? Energy { get; set; }

    public int? Vel { get; set; }

    public virtual ICollection<Club> Clubs { get; set; } = new List<Club>();

    public virtual ICollection<Nation> Nations { get; set; } = new List<Nation>();

    public virtual ICollection<Train> Trains { get; set; } = new List<Train>();
}
