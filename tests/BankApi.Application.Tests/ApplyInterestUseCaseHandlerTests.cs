using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BankApi.Application.Commands.ApplyInterest;
using BankApi.Domain.Entities;
using BankApi.Domain.Interfaces;
using BankApi.Domain.Interfaces.Repositories;
using Moq;
using Xunit;

namespace BankApi.Application.Tests;

public class ApplyInterestUseCaseHandlerTests
{
    [Fact]
    public async Task HandleAsync_ShouldApplyInterest_WhenAccountExistsAndNoInterestAppliedYet()
    {
        var accountId = 1L;
        var generatedId = 999L;
        var initialBalance = 1000m;
        var interestRate = 0.05m;
        var expectedInterest = initialBalance * interestRate;
        var now = DateTime.UtcNow;

        var bankAccount = new BankAccount
        {
            Id = accountId,
            Balance = initialBalance,
            InteresRate = interestRate,
            InterestType = "M",
            BankTransactions = []
        };

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var repoMock = new Mock<IBankAccountRepository>();
        repoMock.Setup(r => r.GetByIdAsync(accountId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(bankAccount);
        unitOfWorkMock.Setup(u => u.BankAccountRepository).Returns(repoMock.Object);
        unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
                      .ReturnsAsync(true);

        var idGeneratorMock = new Mock<IIdGenerator>();
        idGeneratorMock.Setup(i => i.NewId()).Returns(generatedId);

        var handler = new ApplyInterestUseCaseHandler(unitOfWorkMock.Object, idGeneratorMock.Object);

        var command = new ApplyInterestUseCaseCommand(accountId);

        var result = await handler.HandleAsync(command);

        Assert.True(result.IsSuccess);
        Assert.Equal(generatedId, result.Value);
        Assert.Single(bankAccount.BankTransactions);
        var transaction = bankAccount.BankTransactions.First();
        Assert.Equal("I", transaction.TransactionType);
        Assert.Equal(expectedInterest, transaction.Amount);
        Assert.Equal(initialBalance + expectedInterest, bankAccount.Balance);
    }
}
