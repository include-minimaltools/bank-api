using System;
using System.Collections.Generic;
using BankApi.Domain.Entities;

namespace BankApi.Application.DTOs;

public class BaseCustomerDto
{
    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public DateTime BirthDate { get; set; }

    public string Gender { get; set; } = null!;

    public decimal Income { get; set; }
}

public class CustomerRequestDto : BaseCustomerDto;

public class CustomerDto : BaseCustomerDto
{
    public long Id { get; set; }
    
    public ICollection<string> BankTransactions = [];
}
