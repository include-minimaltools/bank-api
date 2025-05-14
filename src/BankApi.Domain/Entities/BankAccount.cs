using System;
using System.Collections.Generic;
using BankApi.Domain.Entities;

namespace BankApi.Domain.Entities;

public partial class BankAccount
{
    public long Id { get; set; }

    public long CustomerId { get; set; }

    public decimal Balance { get; set; }

    public int AccountNumber { get; set; }

    public decimal InteresRate { get; set; }

    public string InterestType { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<BankTransaction> BankTransactions { get; set; } = new List<BankTransaction>();

    public virtual Customer Customer { get; set; } = null!;
}
