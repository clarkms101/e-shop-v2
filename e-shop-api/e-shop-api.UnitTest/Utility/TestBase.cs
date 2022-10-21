using System;
using e_shop_api.DataBase;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace e_shop_api_unit_test.Utility;

public class TestBase : IDisposable
{
    private const string InMemoryConnectionString = "DataSource=:memory:";
    private readonly SqliteConnection _connection;

    protected readonly EShopDbContext FakeEShopDbContext;

    protected TestBase()
    {
        _connection = new SqliteConnection(InMemoryConnectionString);
        _connection.Open();
        var options = new DbContextOptionsBuilder<EShopDbContext>()
            .UseSqlite(_connection)
            .Options;
        FakeEShopDbContext = new EShopDbContext(options);
        FakeEShopDbContext.Database.EnsureCreated();
    }

    public void Dispose()
    {
        _connection.Close();
    }
}