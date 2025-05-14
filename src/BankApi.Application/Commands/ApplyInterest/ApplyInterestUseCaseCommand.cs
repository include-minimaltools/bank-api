using System.ComponentModel.DataAnnotations;

namespace BankApi.Application.Commands.ApplyInterest;

public record ApplyInterestUseCaseCommand(long BankAccountId);