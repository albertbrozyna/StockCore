using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using StockCore.Application.Common.Interfaces;

namespace StockCore.Infrastructure.Persistence.Context;

// To jest instrukcja TYLKO dla terminala (dotnet ef).
// Aplikacja tego nie używa, tylko migrator.
public class StockDbContextFactory : IDesignTimeDbContextFactory<StockDbContext>
{
    public StockDbContext CreateDbContext(string[] args)
    {
        // 1. Tworzymy budowniczego opcji
        var optionsBuilder = new DbContextOptionsBuilder<StockDbContext>();

        // 2. Konfigurujemy połączenie "na sztywno" TYLKO do migracji.
        // Dzięki temu nie musimy się martwić o appsettings.json czy Program.cs
        // Upewnij się, że dane (hasło/user) są takie jak w Dockerze!
        var connectionString = "Host=localhost;Database=stock_db;Username=admin;Password=sekretnehaslo123";

        optionsBuilder.UseNpgsql(connectionString);

        // 3. Zwracamy gotowy kontekst
        return new StockDbContext(optionsBuilder.Options,new DesignTimeUserDetails());
    }

    // Prosta klasa pomocnicza tylko dla migracji
    internal class DesignTimeUserDetails : IUserDetails
    {
        public Guid TenantId { get; set ; }
        public Guid UserId { get; set ; }
    }
}