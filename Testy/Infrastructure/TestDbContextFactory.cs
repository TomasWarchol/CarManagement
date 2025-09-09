using CarManagementApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory; 

namespace Testy.Infrastructure;

public static class TestDbContextFactory
{
    public static CarManagementContext Create(string dbName = "")
    {
        var options = new DbContextOptionsBuilder<CarManagementContext>()
            .UseInMemoryDatabase(string.IsNullOrWhiteSpace(dbName) ? Guid.NewGuid().ToString() : dbName)
            .EnableSensitiveDataLogging()
            .Options;

        var ctx = new CarManagementContext(options);
        return ctx;
    }
}