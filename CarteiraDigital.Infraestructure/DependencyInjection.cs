using CarteiraDigital.Infraestructure.Context;
using CarteiraDigital.Infraestructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CarteiraDigital;

public static class DependencyInjection
{
    public static IServiceCollection AddDatabaseContext(this IServiceCollection services)
    {
        services.AddDbContext<PostgresSQLContext>(options => 
            options.UseNpgsql("Server=localhost;Port=5435;Database=CarteiraDigital_database;User ID=postgres;Password=123;"));


        // Construir o provedor de serviços para criar uma instância do DbContext
        var serviceProvider = services.BuildServiceProvider();

        // Aplicar migrações automaticamente
        using (var scope = serviceProvider.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<PostgresSQLContext>();
            
            try
            {
                dbContext.Database.EnsureCreated();
                dbContext.Database.Migrate();
            }
            catch (Exception ex)
            {
                // Trate exceções conforme necessário
                Console.WriteLine($"Ocorreu um erro ao aplicar as migrações: {ex.Message}");
            }
        }


        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<UserRepository>();

        return services;
    }
}
