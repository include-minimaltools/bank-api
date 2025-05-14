using AutoMapper;
using BankApi.Application.Commands.CreateBankAccount;
using BankApi.Application.DTOs;
using BankApi.Domain.Entities;

namespace BankApi.Application.Mapping;

public class BankAccountProfile : Profile
{
    public BankAccountProfile()
    {
        CreateMap<BankAccountRequestDto, CreateBankAccountUseCaseCommand>();

        CreateMap<BankAccount, BankAccountDto>();
    }
}
