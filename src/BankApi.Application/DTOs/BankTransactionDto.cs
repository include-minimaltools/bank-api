using System.ComponentModel.DataAnnotations;

namespace BankApi.Application.DTOs;

public class BaseBankTransactionDto
{
    [Range(0, int.MaxValue, ErrorMessage = "El monto debe ser mayor a 0")]
    public decimal Amount { get; set; }

    [RegularExpression("D|W", ErrorMessage = "El tipo de transacción debe ser D para depósito o W para retiro")]
    public string TransactionType { get; set; } = null!;
}


public class BankTransactionRequestDto : BaseBankTransactionDto;

public class BankTransactionDto : BaseBankTransactionDto
{
    public long Id { get; set; }

    public long BankAccountId { get; set; }

    public decimal Balance { get; set; }

    public DateTime CreatedAt { get; set; }
}
