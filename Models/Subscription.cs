using System;
using System.Collections.Generic;

namespace demo020925.Models;

public partial class Subscription
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public double Price { get; set; }

    public int DurationPerMonths { get; set; }

    public int? CategoryId { get; set; }

    public virtual SubscriptionsCategory? Category { get; set; }

    public virtual ICollection<SubOrder> SubOrders { get; set; } = new List<SubOrder>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
