using AutoMapper;
using BankApi.Application.Commands.CreateTransaction;
using BankApi.Application.DTOs;
using BankApi.Domain.Entities;

namespace BankApi.Application.Mapping;

public class BankTransactionProfile : Profile
{
    public BankTransactionProfile()
    {
        CreateMap<BankTransactionRequestDto, CreateTransactionUseCaseCommand>();
        CreateMap<BankTransaction, BankTransactionDto>();
    }
}
