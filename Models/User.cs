using System;
using System.Collections.Generic;

namespace demo020925.Models;

public partial class User
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int RoleId { get; set; }

    public int? CurrentSubsriptionId { get; set; }

    public int VisitDurationPerMonts { get; set; }

    public double Discount { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public long? Phone { get; set; }

    public virtual Subscription? CurrentSubsription { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual Role Role { get; set; } = null!;

    public virtual ICollection<SubOrder> SubOrderClients { get; set; } = new List<SubOrder>();

    public virtual ICollection<SubOrder> SubOrderTrainers { get; set; } = new List<SubOrder>();

    public virtual ICollection<UsersBenefit> UsersBenefits { get; set; } = new List<UsersBenefit>();
}
