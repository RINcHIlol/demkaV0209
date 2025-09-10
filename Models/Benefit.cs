using System;
using System.Collections.Generic;

namespace demo020925.Models;

public partial class Benefit
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<UsersBenefit> UsersBenefits { get; set; } = new List<UsersBenefit>();
}
