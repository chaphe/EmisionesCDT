using BackendEmisiones.Data;
using BackendEmisiones.Models;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Net;
using TestEmisionesCDT.Fixture;
using TestEmisionesCDT.Helper;

namespace TestEmisionesCDT.Tests
{
    [Collection("EmisionCDT Collection")]
    public class TestEmpresa
    {
        private readonly WebApplicationFactoryFixture _fixture;
        public TestEmpresa(WebApplicationFactoryFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName ="Crear Empresa OK")]
        public async Task CrearEmpresaOK()
        {
            // Arrange
            var client = _fixture.Client;
            var empresa = new Empresa //DTO que se envia como JSON
            {
                Ciudad = "Bucaramanga",
                Naturaleza = "Juridica",
                RazonSocial = "Hocol",
                Direccion = "Calle 20 Cra 20",
                Identificacion = "1234567",
                Telefono = "310987789",
                NombreContacto = "Juan Perez",
                CargoContacto = "Ing Produccion",
                TelContacto = "312456765",
                FactorGwp = 22
            };

            // Act
            var response = await client.PostAsync("api/Empresa", HttpHelper.GetJsonHttpContent(empresa));
            var empresaStr = response.Content.ReadAsStringAsync();
            var empresaRta = JsonConvert.DeserializeObject<Empresa>(empresaStr.Result);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            using (var scope = _fixture.Factory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DataContext>();
                var empresaDB = context.Empresas.Find(empresaRta.Id);

                // Evalua que el numero de empresas es igual al de la BD
                Assert.Equal(empresaDB.RazonSocial, empresa.RazonSocial);
                Assert.Equal(empresaDB.Ciudad, empresaRta.Ciudad);
                Assert.Equal(empresa.Identificacion, empresaRta.Identificacion);
            }
        }

        [Fact(DisplayName = "Crear Empresa Incompleta")]
        public async Task CrearEmpresaIncompleta()
        {
            // Arrange
            var client = _fixture.Client;
            var empresa = new Empresa //DTO incompleto que se envia como JSON
            {
                Ciudad = "Bucaramanga",
                Naturaleza = "Juridica",            
                Direccion = "Calle 20 Cra 20",
                Identificacion = "1234567",
                Telefono = "310987789",
                NombreContacto = "Juan Perez",
                CargoContacto = "Ing Produccion",
                TelContacto = "312456765",
                FactorGwp = 22
            };

            // Act
            var response = await client.PostAsync("api/Empresa", HttpHelper.GetJsonHttpContent(empresa));

            // Evalua que la respuesta HTTP es 400 (Bad Request)
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact(DisplayName = "Actualizar Empresa OK")]
        public async Task ActualizarEmpresaOK()
        {
            // Arrange
            var client = _fixture.Client;
            Empresa empresaDB = _fixture.GetLastEmpresa();
            /*
            using (var scope = _fixture.Factory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DataContext>();
                empresaDB = context.Empresas.OrderBy(e => e.Id).Last();                
            }
            */

            var empresa = new Empresa //DTO que se envia como JSON
            {
                Id = empresaDB.Id,
                Ciudad = "Barranquilla",
                Naturaleza = "Juridica",
                RazonSocial = "Hocol",
                Direccion = "Calle 20 Cra 20",
                Identificacion = "1234567",
                Telefono = "310987789",
                NombreContacto = "Juan Perez",
                CargoContacto = "Ing Produccion",
                TelContacto = "312456765",
                FactorGwp = 22
            };

            // Act
            var response = await client.PutAsync("api/Empresa", HttpHelper.GetJsonHttpContent(empresa));
            var empresaStr = response.Content.ReadAsStringAsync();
            var empresaRta = JsonConvert.DeserializeObject<Empresa>(empresaStr.Result);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(empresaDB.Identificacion, empresaRta.Identificacion);
            Assert.Equal("Barranquilla", empresaRta.Ciudad);
        }

        [Fact(DisplayName = "Actualizar Empresa No Existente")]
        public async Task ActualizarEmpresaNoExistente()
        {
            // Arrange
            var client = _fixture.Client;

            var empresa = new Empresa //DTO que se envia como JSON
            {
                Id = 10000,
                Ciudad = "Barranquilla",
                Naturaleza = "Juridica",
                RazonSocial = "Hocol",
                Direccion = "Calle 20 Cra 20",
                Identificacion = "1234567",
                Telefono = "310987789",
                NombreContacto = "Juan Perez",
                CargoContacto = "Ing Produccion",
                TelContacto = "312456765",
                FactorGwp = 22
            };

            // Act
            var response = await client.PutAsync("api/Empresa", HttpHelper.GetJsonHttpContent(empresa));
            var textoRta = response.Content.ReadAsStringAsync().Result;
    
            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Contains("no encontrada", textoRta);
        }

        [Fact(DisplayName ="Consultar Empresa OK")]
        public async Task ConsultarEmpresaOK()
        {
            // Arrange
            var client = _fixture.Client;
            // Act
            var response = await client.GetAsync("api/Empresa/1");
            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var empresaStr = response.Content.ReadAsStringAsync();
            var empresaRta = JsonConvert.DeserializeObject<Empresa>(empresaStr.Result);
            Assert.Equal(1, empresaRta.Id);

            using (var scope = _fixture.Factory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DataContext>();
                var empresaDB = context.Empresas.Find(1);
                // Evalua que los atributos corresponden a los de la BD
                Assert.Equal(empresaDB.RazonSocial, empresaRta.RazonSocial);
                Assert.Equal(empresaDB.Identificacion, empresaRta.Identificacion);
                Assert.Equal(empresaDB.Ciudad, empresaRta.Ciudad);

            }
        }

        [Fact(DisplayName = "Consulta Empresa No Existente")]
        public async Task ConsultarEmpresaNoExistente()
        {
            // Arrange
            var client = _fixture.Client;
            // Act
            var response = await client.GetAsync("api/Empresa/1000");

            // Assert

            // Evalua que la respuesta HTTP es 404 (Not Found)
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            var textoRta = response.Content.ReadAsStringAsync().Result;
            // Evalua que el mensaje de respuesta contiene "no encontrada"
            Assert.Contains("no encontrada", textoRta);
        }

        [Fact(DisplayName = "Consultar Conjunto de Empresas")]
        public async Task ConsultaConjuntoEmpresas()
        {
            // Arrange
            var client = _fixture.Client;

            // Act
            var response = await client.GetAsync("api/Empresa/GetAll");

            // Assert
            Assert.Equal("application/json; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            var empresas = JsonConvert.DeserializeObject<List<Empresa>>(content);

            using (var scope = _fixture.Factory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DataContext>();
                // Evalua que el numero de empresas es igual al de la BD
                Assert.Equal(context.Empresas.Count(), empresas.Count());
            }
        }

    }
}
