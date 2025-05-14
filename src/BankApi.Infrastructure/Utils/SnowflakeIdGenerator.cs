using BankApi.Domain.Interfaces;
using IdGen;

namespace BankApi.Infrastructure.Utils;

public class SnowflakeIdGenerator(IIdGenerator<long> generator) : IIdGenerator
{
    private readonly IIdGenerator<long> _generator = generator;

    public long NewId() => _generator.CreateId();
}
