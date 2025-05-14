using BankApi.Application.DTOs;
using BankApi.Application.Interfaces;

public interface ICustomerService : IBaseService<CustomerDto, long, CustomerRequestDto>;