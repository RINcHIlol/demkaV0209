using System;
using System.Collections.Generic;

namespace demo020925.Models;

public partial class UserOrder
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int OrderId { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
