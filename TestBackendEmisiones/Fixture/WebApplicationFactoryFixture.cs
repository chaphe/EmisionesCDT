using BackendEmisiones;
using BackendEmisiones.Data;
using BackendEmisiones.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;


namespace TestEmisionesCDT.Fixture
{
    public class WebApplicationFactoryFixture : IAsyncLifetime
    {
        //private const string _connectionString = @$"Server=localhost\\SQLEXPRESS;Database=CDTNueva;Trusted_Connection=true;TrustServerCertificate=true;";
        private const string _connectionString = "Server=localhost\\SQLEXPRESS;Database=CDTNueva;Trusted_Connection=true;TrustServerCertificate=true;";

        public WebApplicationFactory<Program> Factory { get; private set; }

        public HttpClient Client { get; private set; }

        public WebApplicationFactoryFixture()
        {
            Factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(Services =>
                {
                    Services.RemoveAll(typeof(DbContextOptions<DataContext>));
                    Services.AddDbContext<DataContext>(options =>
                    {
                        options.UseSqlServer(_connectionString);
                    });                 
                });
            });
            Client = Factory.CreateClient();
        }

        async Task IAsyncLifetime.DisposeAsync()
        {
            using (var scope = Factory.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var cntx = scopedServices.GetRequiredService<DataContext>();

                await cntx.Database.EnsureDeletedAsync();
            }
        }

        async Task IAsyncLifetime.InitializeAsync()
        {
            using (var scope = Factory.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var cntx = scopedServices.GetRequiredService<DataContext>();

                await cntx.Database.EnsureCreatedAsync();

                await cntx.Empresas.AddRangeAsync(DataFixture.GetEmpresas(5));
                await cntx.SaveChangesAsync();
                await cntx.Plantas.AddRangeAsync(DataFixture.GetPlantas(20, MaxIdEmpresa(cntx.Empresas)));
                await cntx.SaveChangesAsync();
                await cntx.Sistemas.AddRangeAsync(DataFixture.GetSistemas(40, MaxIdEmpresa(cntx.Empresas)));
                await cntx.SaveChangesAsync();


            }
        }

        public int MaxIdEmpresa(DbSet<Empresa> set)
        {
            return set.Select(x => x.Id)
                    .DefaultIfEmpty() // Maneja el caso cuando no hay elementos en la tabla
                    .Max();
        }

        public int MaxIdPlanta(DbSet<Planta> set)
        {
            return set.Select(x => x.Id)
                    .DefaultIfEmpty() // Maneja el caso cuando no hay elementos en la tabla
                    .Max();
        }

        public int MaxIdSistema(DbSet<Sistema> set)
        {
            return set.Select(x => x.Id)
                    .DefaultIfEmpty() // Maneja el caso cuando no hay elementos en la tabla
                    .Max();
        }
    }
}
