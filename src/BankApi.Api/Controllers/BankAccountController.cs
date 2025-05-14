using System.Threading.Tasks;
using AutoMapper;
using BankApi.Api.Extensions;
using BankApi.Api.Shared;
using BankApi.Application.Commands.CreateBankAccount;
using BankApi.Application.DTOs;
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
}
