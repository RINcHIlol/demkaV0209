using System;
using System.Collections.Generic;

namespace demo020925.Models;

public partial class UsersBenefit
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int BenefitId { get; set; }

    public virtual Benefit Benefit { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
