using System.ComponentModel.DataAnnotations;

namespace BankApi.Application.Commands.CreateTransaction;

public class CreateTransactionUseCaseCommand
{
    public long Id { get; set; }

    public decimal Amount { get; set; }

    [RegularExpression("D|W")]
    public string TransactionType { get; set; } = null!;
}