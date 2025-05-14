using System.Threading.Tasks;
using AutoMapper;
using BankApi.Api.Extensions;
using BankApi.Api.Shared;
using BankApi.Application.Commands.ApplyInterest;
using BankApi.Application.Commands.CreateBankAccount;
using BankApi.Application.Commands.CreateTransaction;
using BankApi.Application.DTOs;
using BankApi.Application.Queries.GetTransactionsByBankAccount;
using BankApi.Infrastructure.Contexts;
using Microsoft.AspNetCore.Mvc;

namespace BankApi.Api.Controllers;

public class BankAccountController(ILogger<BankAccountController> logger, IMapper mapper) : ApiController
{
    private readonly ILogger<BankAccountController> _logger = logger;
    private readonly IMapper _mapper = mapper;

    [HttpPost]
    public async Task<IActionResult> Create(BankAccountRequestDto dto, [FromServices] CreateBankAccountUseCaseHandler createBankAccountUseCaseHandler)
    {
        var command = _mapper.Map<CreateBankAccountUseCaseCommand>(dto);

        var result = await createBankAccountUseCaseHandler.HandleAsync(command, HttpContext.RequestAborted);

        return result.ToActionResult(this);
    }

    [HttpPost("{id}/Transaction")]
    public async Task<IActionResult> CreateTransaction(long id, BankTransactionRequestDto dto, [FromServices] CreateTransactionUseCaseHandler handler, [FromServices] ApplyInterestUseCaseHandler applyInterestUseCaseHandler)
    {
        await applyInterestUseCaseHandler.HandleAsync(new(id));

        var command = _mapper.Map<CreateTransactionUseCaseCommand>(dto);

        command.BankAccountId = id;

        var result = await handler.HandleAsync(command, HttpContext.RequestAborted);

        return result.ToActionResult(this);
    }

    [HttpGet("{id}/Transaction")]
    public async Task<IActionResult> GetTransaction(long id, [FromServices] GetTransactionsByBankAccountHandler handler)
    {
        var result = await handler.HandleAsync(new(id), HttpContext.RequestAborted);

        return result.ToActionResult(this);
    }
}
