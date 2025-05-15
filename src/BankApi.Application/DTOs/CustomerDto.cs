using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BankApi.Domain.Entities;

namespace BankApi.Application.DTOs;

public class BaseCustomerDto
{
    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public DateTime BirthDate { get; set; }

    [RegularExpression("F|M", ErrorMessage = "El genero solamente puede ser M (Masculino) o F (Femenino)")]
    public string Gender { get; set; } = null!;

    [Range(1, int.MaxValue, ErrorMessage = "Debe ingresar un mayor valor a 0")]
    public decimal Income { get; set; }
}

public class CustomerRequestDto : BaseCustomerDto;

public class CustomerDto : BaseCustomerDto
{
    public long Id { get; set; }
    
    public ICollection<string> BankTransactions = [];
}
