using System.Threading.Tasks;
using BankApi.Api.Extensions;
using BankApi.Api.Shared;
using BankApi.Application.DTOs;
using BankApi.Application.Queries.GetBankAccountByCustomer;
using BankApi.Infrastructure.Contexts;
using Microsoft.AspNetCore.Mvc;

namespace BankApi.Api.Controllers;

public class CustomerController(ILogger<CustomerController> logger, ICustomerService customerService) : ApiController
{
    private readonly ILogger<CustomerController> _logger = logger;

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var result = await customerService.GetAllAsync(HttpContext.RequestAborted);
        return result.ToActionResult(this);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(long id)
    {
        var result = await customerService.GetByIdAsync(id, HttpContext.RequestAborted);
        return result.ToActionResult(this);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CustomerRequestDto dto)
    {
        var result = await customerService.CreateAsync(dto, HttpContext.RequestAborted);
        return result.ToActionResult(this);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(long id, CustomerRequestDto dto)
    {
        var result = await customerService.UpdateAsync(id, dto, HttpContext.RequestAborted);
        return result.ToActionResult(this);
    }

    [HttpGet("{id}/BankAccount")]
    public async Task<IActionResult> GetBankAccounts(long id, [FromServices] GetBankAccountByCustomerHandler handler)
    {
        var result = await handler.HandleAsync(new(id), HttpContext.RequestAborted);
        return result.ToActionResult(this);
    }
}
