using BankApi.Domain.Entities;
using BankApi.Domain.Interfaces;
using BankApi.Infrastructure.Contexts;
using BankApi.Infrastructure.Repository;
using BankApi.Infrastructure.Shared;
using IdGen.DependencyInjection;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var seed = builder.Configuration.GetSection("ServerSettings:SnowflakeIdSeed").Get<int>();
builder.Services.AddIdGen(seed);

builder.Services.AddDbContext<BankApiContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IBaseRepository<Customer>, CustomerRepository>();
builder.Services.AddScoped<IBaseRepository<BankAccount>, BankAccountRepository>();
builder.Services.AddScoped<IBaseRepository<BankTransaction>, BankTransactionRepository>();

var app = builder.Build();

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
