using System;
using System.Collections.Generic;
using BankApi.Domain.Entities;

namespace BankApi.Domain.Entities;

public partial class BankTransaction
{
    public long Id { get; set; }

    public long BankAccountId { get; set; }

    public string TransactionType { get; set; } = null!;

    public decimal Amount { get; set; }

    public decimal Balance { get; set; }

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual BankAccount BankAccount { get; set; } = null!;
}
