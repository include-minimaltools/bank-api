using System.ComponentModel.DataAnnotations;

namespace BankApi.Application.DTOs;

public class BaseBankAccountDto
{
    public long CustomerId { get; set; }

    public decimal InteresRate { get; set; }

    [RegularExpression("M|Y", ErrorMessage = "El tipo de interes debe ser M para mensual o Y para anual")]
    public string InterestType { get; set; } = null!;
}

public class BankAccountRequestDto : BaseBankAccountDto
{
    public decimal InicialBalance { get; set; }
}

public class BankAccountDto : BaseBankAccountDto
{
    public long Id { get; set; }

    public long AccountNumber { get; set; }

    public decimal Balance { get; set; }

    public virtual ICollection<BankTransactionDto> BankTransactions { get; set; } = [];
}

