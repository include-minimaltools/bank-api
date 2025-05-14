namespace BankApi.Application.Commands.CreateDeposit;

public class CreateDepositUseCaseCommand
{
    public long BankAccountId { get; set; }

    public decimal Amount { get; set; }
}