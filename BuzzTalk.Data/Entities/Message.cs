using System;
using System.Collections.Generic;

namespace BuzzTalk.Data.Entities;

public partial class Message
{
    public int Id { get; set; }

    public int? FromId { get; set; }

    public int? ToId { get; set; }

    public string Content { get; set; } = null!;

    public DateTime? SentOn { get; set; }

    public bool IsRead { get; set; }

    public int? GroupId { get; set; }

    public virtual User? From { get; set; }

    public virtual Group? Group { get; set; }

    public virtual User? To { get; set; }
}
