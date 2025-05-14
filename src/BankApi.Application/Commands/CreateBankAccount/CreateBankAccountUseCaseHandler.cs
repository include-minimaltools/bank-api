using BankApi.Domain.Entities;
using BankApi.Domain.Interfaces;
using BankApi.Domain.Shared;

namespace BankApi.Application.Commands.CreateBankAccount;

public class CreateBankAccountUseCaseHandler(IUnitOfWork unitOfWork, IIdGenerator idGenerator) : IUseCaseHandler<CreateBankAccountUseCaseCommand, long>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IIdGenerator _idGenerator = idGenerator;

    public async Task<Result<long>> HandleAsync(CreateBankAccountUseCaseCommand request, CancellationToken cancellationToken = default)
    {
        var existCustomer = await _unitOfWork.CustomerRepository.AnyAsync(x => x.Id == request.CustomerId, cancellationToken);

        if (!existCustomer)
            return Result<long>.NotFound("El cliente no se encuentra registrado en el sistema");

        BankAccount account = new()
        {
            Id = _idGenerator.NewId(),
            CustomerId = request.CustomerId,
            AccountNumber = _idGenerator.NewId(),
            CreatedAt = DateTime.UtcNow,
            Balance = request.InitialBalance,
            InteresRate = request.InteresRate,
            InterestType = request.InterestType,
        };

        await _unitOfWork.BankAccountRepository.AddAsync(account, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return account.Id;
    }
}