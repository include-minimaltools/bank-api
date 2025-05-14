using System.Numerics;
using BankApi.Api.Middleware;
using BankApi.Api.Utils;
using BankApi.Application.Commands.ApplyInterest;
using BankApi.Application.Commands.CreateBankAccount;
using BankApi.Application.Commands.CreateTransaction;
using BankApi.Application.Queries.GetBankAccountByCustomer;
using BankApi.Application.Queries.GetTransactionsByBankAccount;
using BankApi.Application.Services;
using BankApi.Domain.Entities;
using BankApi.Domain.Interfaces;
using BankApi.Domain.Interfaces.Repositories;
using BankApi.Infrastructure.Contexts;
using BankApi.Infrastructure.Data;
using BankApi.Infrastructure.Repository;
using BankApi.Infrastructure.Shared;
using BankApi.Infrastructure.Utils;
using IdGen.DependencyInjection;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var seed = builder.Configuration.GetSection("ServerSettings:SnowflakeIdSeed").Get<int>();
builder.Services.AddIdGen(seed);

builder.Services.AddDbContext<BankApiContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new LongToStringConverter());
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddTransient<IIdGenerator, SnowflakeIdGenerator>();

builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IBankAccountRepository, BankAccountRepository>();
builder.Services.AddScoped<IBankTransactionRepository, BankTransactionRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ICustomerService, CustomerService>();


builder.Services.AddScoped<CreateBankAccountUseCaseHandler>();
builder.Services.AddScoped<CreateTransactionUseCaseHandler>();
builder.Services.AddScoped<GetBankAccountByCustomerHandler>();
builder.Services.AddScoped<GetTransactionsByBankAccountHandler>();
builder.Services.AddScoped<ApplyInterestUseCaseHandler>();

builder.Services.AddTransient<GlobalExceptionHandler>();


var app = builder.Build();

app.UseMiddleware<GlobalExceptionHandler>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
