using System;
using System.Collections.Generic;

namespace BuzzTalk.Data.Entities;

public partial class Group
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string Icon { get; set; } = null!;

    public int? CreatedBy { get; set; }

    public DateTime CreatedTime { get; set; }

    public bool IsDeleted { get; set; }

    public string Guid { get; set; } = null!;

    public virtual User? CreatedByNavigation { get; set; }

    public virtual ICollection<GroupUser> GroupUsers { get; set; } = new List<GroupUser>();

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();
}
