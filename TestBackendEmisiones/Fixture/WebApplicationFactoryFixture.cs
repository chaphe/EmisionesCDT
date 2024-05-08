using BackendEmisiones;
using BackendEmisiones.Data;
using BackendEmisiones.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Numerics;


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
                await cntx.Sistemas.AddRangeAsync(DataFixture.GetSistemas(80, MaxIdPlanta(cntx.Plantas)));
                await cntx.SaveChangesAsync();
            }
        }

        private int MaxIdEmpresa(DbSet<Empresa> set)
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

        public Empresa GetLastEmpresa()
        {
            using (var scope = Factory.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var cntx = scopedServices.GetRequiredService<DataContext>();
                return cntx.Empresas.OrderBy(e => e.Id).Last();
            }
        }

        public Empresa GetRandomEmpresa()
        {
            using (var scope = Factory.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var cntx = scopedServices.GetRequiredService<DataContext>();

                var randomElement = cntx.Empresas.OrderBy(c => Guid.NewGuid())
                                        .FirstOrDefault();
                return randomElement;
            }
        }

        public Planta GetLastPlanta()
        {
            using (var scope = Factory.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var cntx = scopedServices.GetRequiredService<DataContext>();
                return cntx.Plantas.OrderBy(p => p.Id).Last();
            }
        }

        public Planta GetRandomPlanta()
        {
            using (var scope = Factory.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var cntx = scopedServices.GetRequiredService<DataContext>();
                var randomElement = cntx.Plantas.OrderBy(c => Guid.NewGuid())
                                        .FirstOrDefault();
                return randomElement;
            }
        }

        public Planta GetPlantaPorId(int id)
        {
            using (var scope = Factory.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var cntx = scopedServices.GetRequiredService<DataContext>();
                return cntx.Plantas.Find(id);
            }
        }

        public List<Planta> GetPlantasPorEmpresa(int idEmpresa)
        {
            using (var scope = Factory.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var cntx = scopedServices.GetRequiredService<DataContext>();

                return cntx.Plantas.Where(p => p.EmpresaId == idEmpresa).ToList();
            }
        }

        public Sistema GetSistemaPorId(int id)
        {
            using (var scope = Factory.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var cntx = scopedServices.GetRequiredService<DataContext>();
                return cntx.Sistemas.Find(id);
            }
        }

        public Sistema GetRandomSistema()
        {
            using (var scope = Factory.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var cntx = scopedServices.GetRequiredService<DataContext>();
                var randomElement = cntx.Sistemas.OrderBy(c => Guid.NewGuid())
                                        .FirstOrDefault();
                return randomElement;
            }
        }

        public List<Sistema> GetSistemasPorPlanta(int idPlanta)
        {
            using (var scope = Factory.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var cntx = scopedServices.GetRequiredService<DataContext>();

                return cntx.Sistemas.Where(p => p.PlantaId == idPlanta).ToList();
            }
        }
    }
}
