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


namespace TestBackendEmisiones.Fixture
{
    public class WebApplicationFactoryFixture : IAsyncLifetime
    {
        //private const string _connectionString = @$"Server=localhost\\SQLEXPRESS;Database=CDTNueva;Trusted_Connection=true;TrustServerCertificate=true;";
        private const string _connectionString = "Server=localhost\\SQLEXPRESS;Database=CDTNueva;Trusted_Connection=true;TrustServerCertificate=true;";

        const int NumeroEmpresas = 10;
        const int NumeroPlantas = 30;
        const int NumeroSistemas = 90;
        const int NumeroEmisionesCombustion = 1800;
        const int NumeroEmisionesFugitivas = 1800;
        const int NumeroEvidencias = 200;


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

                await cntx.FactoresEmision.AddRangeAsync(CrearFactoresEmision());
                await cntx.SaveChangesAsync();
                await cntx.TipoFuente.AddRangeAsync(CrearTiposFuente());
                await cntx.SaveChangesAsync();
                await cntx.Empresas.AddRangeAsync(DataFixture.GetEmpresas(NumeroEmpresas));
                await cntx.SaveChangesAsync();
                await cntx.Plantas.AddRangeAsync(DataFixture.GetPlantas(NumeroPlantas, NumeroEmpresas));
                await cntx.SaveChangesAsync();
                await cntx.Sistemas.AddRangeAsync(DataFixture.GetSistemas(NumeroSistemas, NumeroPlantas));
                await cntx.SaveChangesAsync();
                await cntx.EmisionesCombustion.AddRangeAsync(DataFixture.GetEmisionesCombustion(NumeroEmisionesCombustion, NumeroSistemas));
                await cntx.SaveChangesAsync();
                await cntx.EmisionesFugitivas.AddRangeAsync(DataFixture.GetEmisionesFugitivas(NumeroEmisionesFugitivas, NumeroSistemas));
                await cntx.SaveChangesAsync();
                await cntx.Evidencias.AddRangeAsync(DataFixture.GetEvidencias(NumeroEvidencias, NumeroEmisionesFugitivas));
                await cntx.SaveChangesAsync();
            }
        }

        private List<TipoFuente> CrearTiposFuente()
        {
            var lista = new List<TipoFuente>();
            lista.Add(new TipoFuente
            {
                IdClasificacion = 1,
                Nombre = "Motor de compresor",
                TipoEmision = 2
            });
            lista.Add(new TipoFuente
            {
                IdClasificacion = 1,
                Nombre = "Motor de generador",
                TipoEmision = 2
            });
            lista.Add(new TipoFuente
            {
                IdClasificacion = 1,
                Nombre = "Horno",
                TipoEmision = 1
            });
            lista.Add(new TipoFuente
            {
                IdClasificacion = 1,
                Nombre = "Caldera Calentador",
                TipoEmision = 1
            });
            lista.Add(new TipoFuente
            {
                IdClasificacion = 1,
                Nombre = "Tea",
                TipoEmision = 1
            });
            return lista;
        }
        private List<FactorEmision> CrearFactoresEmision()
        {
            var lista = new List<FactorEmision>();
            lista.Add(new FactorEmision
            {
                NombreGas = "Gas Natural",
                ValorCh4fugitivas = 11.7m,
                ValorCo2fugitivas = 2.2m,
                ValorCo2combustion = 294.6m
            });
            lista.Add(new FactorEmision
            {
                NombreGas = "Gas Rico Apiai",
                ValorCh4fugitivas = 15.6m,
                ValorCo2fugitivas = 2.8m,
                ValorCo2combustion = 393.4m
            });
            lista.Add(new FactorEmision
            {
                NombreGas = "Gas Seco Apiai",
                ValorCh4fugitivas = 15.8m,
                ValorCo2fugitivas = 2.1m,
                ValorCo2combustion = 396.6m
            });
            lista.Add(new FactorEmision
            {
                NombreGas = "Gas Seco Cusiana",
                ValorCh4fugitivas = 17.2m,
                ValorCo2fugitivas = 2.8m,
                ValorCo2combustion = 398.8m
            });
            return lista;
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

        public Empresa GetEmpresaPorId(int id)
        {
            using (var scope = Factory.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var cntx = scopedServices.GetRequiredService<DataContext>();
                return cntx.Empresas.Find(id);
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

        public EmisionCombustion GetEmisionCombustionPorId(int id)
        {
            using (var scope = Factory.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var cntx = scopedServices.GetRequiredService<DataContext>();
                return cntx.EmisionesCombustion.Find(id);
            }
        }

        public EmisionCombustion GetRandomEmisionCombustion()
        {
            using (var scope = Factory.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var cntx = scopedServices.GetRequiredService<DataContext>();
                var randomElement = cntx.EmisionesCombustion.OrderBy(c => Guid.NewGuid())
                                        .FirstOrDefault();
                return randomElement;
            }
        }

        public List<EmisionCombustion> GetEmisionCombustionPorSistema(int idSistema)
        {
            using (var scope = Factory.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var cntx = scopedServices.GetRequiredService<DataContext>();

                return cntx.EmisionesCombustion.Where(e => e.SistemaId == idSistema).ToList();
            }
        }

        public EmisionFugitiva GetEmisionFugitivaPorId(int id)
        {
            using (var scope = Factory.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var cntx = scopedServices.GetRequiredService<DataContext>();
                return cntx.EmisionesFugitivas.Find(id);
            }
        }

        public EmisionFugitiva GetRandomEmisionFugitiva()
        {
            using (var scope = Factory.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var cntx = scopedServices.GetRequiredService<DataContext>();
                var randomElement = cntx.EmisionesFugitivas.OrderBy(c => Guid.NewGuid())
                                        .FirstOrDefault();
                return randomElement;
            }
        }

        public List<EmisionFugitiva> GetEmisionFugitivaPorSistema(int idSistema)
        {
            using (var scope = Factory.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var cntx = scopedServices.GetRequiredService<DataContext>();

                return cntx.EmisionesFugitivas.Where(e => e.SistemaId == idSistema).ToList();
            }
        }

        public Evidencia GetEvidenciaPorId(int id)
        {
            using (var scope = Factory.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var cntx = scopedServices.GetRequiredService<DataContext>();
                return cntx.Evidencias.Find(id);
            }
        }

        public Evidencia GetRandomEvidencia()
        {
            using (var scope = Factory.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var cntx = scopedServices.GetRequiredService<DataContext>();
                var randomElement = cntx.Evidencias.OrderBy(c => Guid.NewGuid())
                                        .FirstOrDefault();
                return randomElement;
            }
        }

        public List<Evidencia> GetEvidenciasPorEmision(int idEmision)
        {
            using (var scope = Factory.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var cntx = scopedServices.GetRequiredService<DataContext>();

                return cntx.Evidencias.Where(p => p.EmisionFugitivaId == idEmision).ToList();
            }
        }

    }
}
