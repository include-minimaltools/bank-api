using AutoMapper;
using BankApi.Application.DTOs;
using BankApi.Domain.Entities;

namespace BankApi.Application.Mapping;

public class CustomerProfile : Profile
{
    public CustomerProfile()
    {
        CreateMap<Customer, CustomerDto>();
        CreateMap<CustomerDto, Customer>();

        CreateMap<Customer, CustomerRequestDto>();
        CreateMap<CustomerRequestDto, Customer>();
    }
}
