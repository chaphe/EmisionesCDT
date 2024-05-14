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
using static System.Runtime.InteropServices.JavaScript.JSType;


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
                await cntx.ReporteMensual.AddRangeAsync(CrearReportesMensual());
                await cntx.SaveChangesAsync();
                await cntx.ReporteMensualGas.AddRangeAsync(CrearReportesMensualGas());
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

        private List<ReporteMensual> CrearReportesMensual()
        {
            var lista = new List<ReporteMensual>();
            lista.Add(new ReporteMensual
            {
                EmpresaId = 1,
                Empresa = "Ecopetrol",
                PlantaId = 1,
                Planta = "Acacias",
                Anho = 2023,
                Mes = 1
            });
            return lista;
        }

        private List<ReporteMensualGas> CrearReportesMensualGas()
        {
            var lista = new List<ReporteMensualGas>();
            lista.Add(new ReporteMensualGas
            {
                EmpresaId = 1,
                Empresa = "Ecopetrol",
                PlantaId = 1,
                Planta = "Acacias",
                GasId = 1,
                Gas = "Gas Seco Apiai",
                Anho = 2023,
                Mes = 1
            });
            return lista;
        }

        private List<TipoFuente> CrearTiposFuente()
        {
            var lista = new List<TipoFuente>();
            lista.Add(crearTipoFuente(1, "Motor de compresor", 2));
            lista.Add(crearTipoFuente(1,"Motor de generador",2));       
            lista.Add(crearTipoFuente(1, "Horno", 1));
            lista.Add(crearTipoFuente(1, "Caldera Calentador", 1));
            lista.Add(crearTipoFuente(1, "Tea", 1));
            return lista;
        }

        private TipoFuente crearTipoFuente(int idClasificiacion, string nombre, int tipoEmision)
        {
            return new TipoFuente
            {
                IdClasificacion = idClasificiacion,
                Nombre = nombre,
                TipoEmision = tipoEmision
            };
        }
        
        private List<FactorEmision> CrearFactoresEmision()
        {
            var lista = new List<FactorEmision>();
            lista.Add(crearFactorEmision("Gas Natural", 11.7m, 2.2m, 294.6m));
            lista.Add(crearFactorEmision("Gas Rico Apiai", 15.6m, 2.8m, 393.4m));
            lista.Add(crearFactorEmision("Gas Seco Apiai", 15.8m, 2.1m, 396.6m));
            lista.Add(crearFactorEmision("Gas Seco Cusiana", 17.2m, 2.8m, 398.8m));
            return lista;
        }

        private FactorEmision crearFactorEmision(string gas, decimal ch4fug, decimal co2fug, decimal co2com)
        {
            return new FactorEmision
            {
                NombreGas = gas,
                ValorCh4fugitivas = ch4fug,
                ValorCo2fugitivas = co2fug,
                ValorCo2combustion = co2com
            };
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
