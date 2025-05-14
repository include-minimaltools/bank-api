using System;
using System.Collections.Generic;
using BankApi.Domain.Entities;

namespace BankApi.Domain.Entities;

public partial class Customer
{
    public long Id { get; set; }

    public string FirstName { get; set; } = null!;

    public byte[] LastName { get; set; } = null!;

    public DateTime BirthDate { get; set; }

    public string Gender { get; set; } = null!;

    public decimal Income { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<BankAccount> BankAccounts { get; set; } = new List<BankAccount>();
}
