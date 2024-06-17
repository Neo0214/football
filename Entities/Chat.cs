using System;
using System.Collections.Generic;

namespace football.Entities;

public partial class Chat
{
    public int _1Id { get; set; }

    public int _2Id { get; set; }

    public int ChatId { get; set; }

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();

    public virtual User _1 { get; set; } = null!;

    public virtual User _2 { get; set; } = null!;
}
