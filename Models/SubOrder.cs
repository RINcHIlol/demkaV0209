using System;
using System.Collections.Generic;

namespace demo020925.Models;

public partial class SubOrder
{
    public int Id { get; set; }

    public int SubId { get; set; }

    public int OrderId { get; set; }

    public int TrainerId { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Subscription Sub { get; set; } = null!;

    public virtual User Trainer { get; set; } = null!;
}
