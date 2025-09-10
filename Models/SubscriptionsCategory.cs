using System;
using System.Collections.Generic;

namespace demo020925.Models;

public partial class SubscriptionsCategory
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
}
