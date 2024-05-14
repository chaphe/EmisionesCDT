using BackendEmisiones.Models;
using Newtonsoft.Json;
using System.Net;
using TestBackendEmisiones.Fixture;
using TestBackendEmisiones.Helper;

namespace TestBackendEmisiones.Tests
{
    [Collection("Backend EmisionCDT Collection")]
    public class TestPlanta
    {
        private readonly WebApplicationFactoryFixture _fixture;
        public TestPlanta(WebApplicationFactoryFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = "Crear Planta OK")]
        public async Task CrearPlantaOK()
        {
            // Arrange
            var client = _fixture.Client;
            var planta = CrearPlantaDTO();
            var content = HttpHelper.GetJsonHttpContent(planta);

            // Act
            var response = await client.PostAsync("api/Planta", content);
            var plantaStr = response.Content.ReadAsStringAsync();
            var plantaRta = JsonConvert.DeserializeObject<Planta>(plantaStr.Result);

            // Assert

            // Evalua que el çodigo HTTP de respuesta es 200 (OK)
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var plantaDB = _fixture.GetPlantaPorId(plantaRta.Id);

            // Valida que los atribitos son iguales a la BD
            Assert.Equal(plantaDB.Nombre, plantaRta.Nombre);
            Assert.Equal(plantaDB.Ciudad, plantaRta.Ciudad);
            Assert.Equal(plantaDB.EmpresaId, plantaRta.EmpresaId);
        }

        [Fact(DisplayName = "Crear Planta Empresa No Existente")]
        public async Task CrearPlantaEmpresaNoExistente()
        {
            // Arrange
            var client = _fixture.Client;
            var planta = new Planta //DTO que se envia como JSON
            {
                Nombre = "Planta de Acacias",
                Ciudad = "Acacias",
                EmpresaId = 10000
            };

            // Act
            var content = HttpHelper.GetJsonHttpContent(planta);
            var response = await client.PostAsync("api/Planta", content);
            var textoStr = response.Content.ReadAsStringAsync().Result;


            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Contains("No existe", textoStr);
        }

        [Fact(DisplayName = "Actualizar Planta OK")]
        public async Task ActualizarPlantaOK()
        {
            // Arrange
            var client = _fixture.Client;
            Planta plantaDB = _fixture.GetRandomPlanta();

            var planta = new Planta //DTO que se envia como JSON
            {
                Id = plantaDB.Id,
                Nombre = "Planta de Acacias",
                Ciudad = "Barranquilla",
                EmpresaId = plantaDB.EmpresaId
            };

            // Act
            var content = HttpHelper.GetJsonHttpContent(planta);
            var response = await client.PutAsync("api/Planta", content);
            var plantaStr = response.Content.ReadAsStringAsync();
            var plantaRta = JsonConvert.DeserializeObject<Planta>(plantaStr.Result);
            plantaDB = _fixture.GetPlantaPorId(plantaDB.Id);
            
            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(plantaDB.Nombre, plantaRta.Nombre);
            Assert.Equal(plantaDB.Ciudad, plantaRta.Ciudad);
            Assert.Equal(plantaDB.EmpresaId, plantaRta.EmpresaId);
            Assert.Equal(planta.Nombre, plantaRta.Nombre);
            Assert.Equal(planta.Ciudad, plantaRta.Ciudad);
        }

        [Fact(DisplayName = "Actualizar Planta No Existente")]
        public async Task ActualizarPlantaNoExistente()
        {
            // Arrange
            var client = _fixture.Client;          

            var planta = new Planta //DTO que se envia como JSON
            {
                Id = 10000, //ID no exixtente
                Nombre = "Planta de Acacias",
                Ciudad = "Barranquilla",
                EmpresaId = 1
            };

            // Act
            var content = HttpHelper.GetJsonHttpContent(planta);
            var response = await client.PutAsync("api/Planta", content);
            var textoRta = response.Content.ReadAsStringAsync().Result;

            // Validad que la respuesta HTTP es 404 (Not Found)
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            // Valida que eL texto de respuesta contiene "no encontrada"
            Assert.Contains("no encontrada", textoRta);
        }

        [Fact(DisplayName = "Consultar Planta OK")]
        public async Task ConsultarPlantaOK()
        {
            // Arrange
            var client = _fixture.Client;
            var plantaDB = _fixture.GetRandomPlanta();
            // Act
            var response = await client.GetAsync("api/Planta/" + plantaDB.Id);
            // Assert
            var plantaStr = response.Content.ReadAsStringAsync();
            var plantaRta = JsonConvert.DeserializeObject<Planta>(plantaStr.Result);

            // Valida que el código de respuesta sea 200 (OK)
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            // Valida que los atributos corresponden a los de la BD
            Assert.Equal(plantaDB.Nombre, plantaRta.Nombre);
            Assert.Equal(plantaDB.Ciudad, plantaRta.Ciudad);
            Assert.Equal(plantaDB.Id, plantaRta.Id);
        }

        [Fact(DisplayName = "Consultar Planta No Existente")]
        public async Task ConsultarPlantaNoExistente()
        {
            // Arrange
            var client = _fixture.Client;
            // Act
            var response = await client.GetAsync("api/Planta/10000");
            // Assert
            var textoRta = response.Content.ReadAsStringAsync().Result;

            // Valida que el código de respuesta sea 404 (No encontrado)
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Contains("no encontrada", textoRta);
        }

        [Fact(DisplayName = "Consultar Conjunto de Plantas")]
        public async Task ConsultaConjuntoPlantas()
        {
            // Arrange
            var client = _fixture.Client;
            var empresaDB = _fixture.GetRandomEmpresa();


            // Act
            var response = await client.GetAsync("api/Planta/GetSet/" + empresaDB.Id);

            // Assert
            // Valida que el codigo de respuesta sea 200 OK
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            var plantasRta = JsonConvert.DeserializeObject<List<Planta>>(content);
            var plantasDB = _fixture.GetPlantasPorEmpresa(empresaDB.Id);
            // Evalua que el numero de empresas es igual al de la BD
            Assert.Equal(plantasDB.Count(), plantasRta.Count());
        }

        private Planta CrearPlantaDTO(int id = 1, int empresaId = 1)
        {
            return new Planta
            {
                Id = id,
                Nombre = "Planta de Acacias",
                Ciudad = "Acacias",
                EmpresaId = empresaId
            };
        }
    }
}
