using System.ComponentModel.DataAnnotations;

namespace BankApi.Application.Commands.CreateTransaction;

public class CreateTransactionUseCaseCommand
{
    public long BankAccountId { get; set; }

    public decimal Amount { get; set; }

    public string TransactionType { get; set; } = null!;
}