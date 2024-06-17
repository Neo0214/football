using System;
using System.Collections.Generic;

namespace football.Entities;

public partial class Message
{
    public int MessageId { get; set; }

    public int? From { get; set; }

    public string? Content { get; set; }

    public int? ChatId { get; set; }

    public virtual Chat? Chat { get; set; }

    public virtual User? FromNavigation { get; set; }
}
