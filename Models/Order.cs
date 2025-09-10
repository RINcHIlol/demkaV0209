using System;
using System.Collections.Generic;

namespace demo020925.Models;

public partial class Order
{
    public int Id { get; set; }

    public double Price { get; set; }

    public DateTime OrderTime { get; set; }

    public int CustomerId { get; set; }

    public virtual User Customer { get; set; } = null!;

    public virtual ICollection<SubOrder> SubOrders { get; set; } = new List<SubOrder>();
}
