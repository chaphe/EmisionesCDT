using BackendEmisiones.Data;
using BackendEmisiones.Models;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Net;
using TestBackendEmisiones.Fixture;
using TestBackendEmisiones.Helper;

namespace TestBackendEmisiones.Tests
{
    [Collection("Backend EmisionCDT Collection")]
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
            var empresa = CrearEmpresaDTO();

            // Act
            var content = HttpHelper.GetJsonHttpContent(empresa);
            var response = await client.PostAsync("api/Empresa", content);
            var empresaStr = response.Content.ReadAsStringAsync();
            var empresaRta = JsonConvert.DeserializeObject<Empresa>(empresaStr.Result);
            var empresaDB = _fixture.GetEmpresaPorId(empresaRta.Id);

            // Assert
            // Valida que la respuesta HTTP es 200 (OK)
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            // Valida que los atributos sean iguales a la BD
            Assert.Equal(empresaDB.RazonSocial, empresa.RazonSocial);
            Assert.Equal(empresaDB.Ciudad, empresa.Ciudad);
            Assert.Equal(empresaDB.Identificacion, empresa.Identificacion);
            Assert.Equal(empresaDB.NombreContacto, empresa.NombreContacto);
        }

        [Fact(DisplayName = "Crear Empresa Incompleta")]
        public async Task CrearEmpresaIncompleta()
        {
            // Arrange
            var client = _fixture.Client;
            var empresa = CrearEmpresaDTO();
            empresa.RazonSocial = null;

            // Act
            var content = HttpHelper.GetJsonHttpContent(empresa);
            var response = await client.PostAsync("api/Empresa", content);

            // Valida que la respuesta HTTP es 400 (Bad Request)
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact(DisplayName = "Actualizar Empresa OK")]
        public async Task ActualizarEmpresaOK()
        {
            // Arrange
            var client = _fixture.Client;
            var empresaDB = _fixture.GetRandomEmpresa();
            var empresa = CrearEmpresaDTO(empresaDB.Id);

            // Act
            var content = HttpHelper.GetJsonHttpContent(empresa);
            var response = await client.PutAsync("api/Empresa", content);
            var empresaStr = response.Content.ReadAsStringAsync();
            var empresaRta = JsonConvert.DeserializeObject<Empresa>(empresaStr.Result);
            empresaDB = _fixture.GetEmpresaPorId(empresa.Id);
            
            // Assert
            // Valida que la respuesta HTTP es 200 (OK)
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            // Valida que los atributos se actualziaron
            Assert.Equal(empresaDB.RazonSocial, empresa.RazonSocial);
            Assert.Equal(empresaDB.Ciudad, empresa.Ciudad);
            Assert.Equal(empresaDB.Identificacion, empresa.Identificacion);
            Assert.Equal(empresaDB.NombreContacto, empresa.NombreContacto);
        }

        [Fact(DisplayName = "Actualizar Empresa No Existente")]
        public async Task ActualizarEmpresaNoExistente()
        {
            // Arrange
            var client = _fixture.Client;
            var empresa = CrearEmpresaDTO(10000);

            // Act
            var content = HttpHelper.GetJsonHttpContent(empresa);
            var response = await client.PutAsync("api/Empresa", content);
            var textoRta = response.Content.ReadAsStringAsync().Result;

            // Assert

            // Valida que la respuesta HTTP es 404 (NotFound)
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Contains("no encontrada", textoRta);
        }

        [Fact(DisplayName ="Consultar Empresa OK")]
        public async Task ConsultarEmpresaOK()
        {
            // Arrange
            var client = _fixture.Client;
            var empresaDB = _fixture.GetRandomEmpresa();
            // Act
            var response = await client.GetAsync("api/Empresa/" + empresaDB.Id);
            var empresaStr = response.Content.ReadAsStringAsync();
            var empresaRta = JsonConvert.DeserializeObject<Empresa>(empresaStr.Result);

            // Assert
            // Valida que la respuesta HTTP es 200 (OK)
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            // Valida que los atributos corresponden a la BD
            Assert.Equal(empresaDB.RazonSocial, empresaRta.RazonSocial);
            Assert.Equal(empresaDB.Ciudad, empresaRta.Ciudad);
            Assert.Equal(empresaDB.Identificacion, empresaRta.Identificacion);
            Assert.Equal(empresaDB.NombreContacto, empresaRta.NombreContacto);
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
            var content = await response.Content.ReadAsStringAsync();
            var empresas = JsonConvert.DeserializeObject<List<Empresa>>(content);

            // Assert
            // Valida que la respuesta HTTP es 200 (OK)
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            using (var scope = _fixture.Factory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DataContext>();
                // Valida que el numero de empresas es igual al de la BD
                Assert.Equal(context.Empresas.Count(), empresas.Count());
            }
        }

        private Empresa CrearEmpresaDTO(int id=1)
        {
            return new Empresa 
            {
                Id = id,
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
        }

    }
}
