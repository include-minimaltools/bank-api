namespace BankApi.Application.DTOs;

public class BaseBankTransactionDto
{
    public decimal Amount { get; set; }

    public string TransactionType { get; set; } = null!;
}


public class BankTransactionRequestDto : BaseBankTransactionDto;

public class BankTransactionDto : BaseBankTransactionDto
{
    public long Id { get; set; }

    public long BankAccountId { get; set; }
    
    public decimal Balance { get; set; }
}
