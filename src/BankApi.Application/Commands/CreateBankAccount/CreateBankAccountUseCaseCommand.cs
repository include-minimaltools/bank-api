namespace BankApi.Application.Commands.CreateBankAccount;

public class CreateBankAccountUseCaseCommand
{
    public long CustomerId { get; set; }

    public decimal InitialBalance { get; set; }

    public decimal InteresRate { get; set; }

    public string InterestType { get; set; } = null!;
}