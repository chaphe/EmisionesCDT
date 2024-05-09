using BackendEmisiones.Models;
using Newtonsoft.Json;
using System.Net;
using TestBackendEmisiones.Fixture;
using TestBackendEmisiones.Helper;

namespace TestBackendEmisiones.Tests
{
    [Collection("Backend EmisionCDT Collection")]
    public class TestEmisionesCombustion
    {
        private readonly WebApplicationFactoryFixture _fixture;

        public TestEmisionesCombustion(WebApplicationFactoryFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = "Crear Emision Combustion OK")]
        public async Task CrearEmisionCombustionOK()
        {
            // Arrange
            var client = _fixture.Client;
            var emision = CrearEmisionCombustionDTO();
            var content = HttpHelper.GetJsonHttpContent(emision);

            // Act
            var response = await client.PostAsync("api/EmisionCombustion", content);
            var emisionStr = response.Content.ReadAsStringAsync();
            var emisionRta = JsonConvert.DeserializeObject<EmisionCombustion>(emisionStr.Result);
            var emisionDB = _fixture.GetEmisionCombustionPorId(emisionRta.Id);
            // Assert

            // Valida que el çodigo HTTP de respuesta es 200 (OK)
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            // Valida que los atributos en la BD son iguales al DTO
            Assert.Equal(emisionDB.Nombre, emisionRta.Nombre);
            Assert.Equal(emisionDB.Descripcion, emisionRta.Descripcion);
            Assert.Equal(emisionDB.Tag, emisionRta.Tag);
            Assert.Equal(emisionDB.HorasOperacion, emisionRta.HorasOperacion);
            Assert.Equal(emisionDB.EficienciaCombustion, emisionRta.EficienciaCombustion);
            Assert.Equal(emisionDB.SistemaId, emisionRta.SistemaId);
            Assert.Equal(emisionDB.TipoFuenteId, emisionRta.TipoFuenteId);
            Assert.Equal(emisionDB.FactorEmisionId, emisionRta.FactorEmisionId);
        }

        [Fact(DisplayName = "Crear Emision Combustion Sistema No Existente")]
        public async Task CrearEmisionCombustionSistemaNoExistente()
        {
            // Arrange
            var client = _fixture.Client;
            var emision = CrearEmisionCombustionDTO(1, 10000);

            // Act
            var content = HttpHelper.GetJsonHttpContent(emision);
            var response = await client.PostAsync("api/EmisionCombustion", content);
            var textoStr = response.Content.ReadAsStringAsync().Result;


            // Assert
            // Valida que el çodigo HTTP de respuesta es 404 (Not Found)
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            //Valida que el mensaje regresado contenga "No existe"
            Assert.Contains("No existe", textoStr);
        }

        private Sistema CrearSistemaDTO(int v1=1, int v2=1)
        {
            throw new NotImplementedException();
        }

        [Fact(DisplayName = "Actualizar Emision Combustion OK")]
        public async Task ActualizarEmisionCombustionOK()
        {
            // Arrange
            var client = _fixture.Client;
            var emisionDB = _fixture.GetRandomEmisionCombustion();
            var emision = CrearEmisionCombustionDTO(emisionDB.Id, emisionDB.SistemaId);

            // Act
            var content = HttpHelper.GetJsonHttpContent(emision);
            var response = await client.PutAsync("api/EmisionCombustion", content);
            var emisionStr = response.Content.ReadAsStringAsync();
            var emisionRta = JsonConvert.DeserializeObject<EmisionCombustion>(emisionStr.Result);
            emisionDB = _fixture.GetEmisionCombustionPorId(emision.Id);

            // Assert
            // Valida que la respuesta HTTP es 200 (OK)
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            // Valida que los atributos fueron actualizados correctamente
            Assert.Equal(emisionDB.Nombre, emisionRta.Nombre);
            Assert.Equal(emisionDB.Descripcion, emisionRta.Descripcion);
            Assert.Equal(emisionDB.Tag, emisionRta.Tag);
            Assert.Equal(emisionDB.HorasOperacion, emisionRta.HorasOperacion);
            Assert.Equal(emisionDB.EficienciaCombustion, emisionRta.EficienciaCombustion);
            Assert.Equal(emisionDB.SistemaId, emisionRta.SistemaId);
            Assert.Equal(emisionDB.TipoFuenteId, emisionRta.TipoFuenteId);
            Assert.Equal(emisionDB.FactorEmisionId, emisionRta.FactorEmisionId);
        }

        [Fact(DisplayName = "Actualizar Emision Combustion No Existente")]
        public async Task ActualizarEmisionCombustionNoExistente()
        {
            // Arrange
            var client = _fixture.Client;
            var emision = CrearEmisionCombustionDTO(10000);

            // Act
            var content = HttpHelper.GetJsonHttpContent(emision);
            var response = await client.PutAsync("api/EmisionCombustion", content);
            var textoRta = response.Content.ReadAsStringAsync().Result;

            // Assert
            // Valida que el çodigo HTTP de respuesta es 404 (Not Found)
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            // Valida que el mensaje de error contiene "no encontrada"
            Assert.Contains("no encontrada", textoRta);
        }

        [Fact(DisplayName = "Consultar Emision Combustion OK")]
        public async Task ConsultarEmisionCombustionOK()
        {
            // Arrange
            var client = _fixture.Client;
            var emisionDB = _fixture.GetRandomEmisionCombustion();
            // Act
            var response = await client.GetAsync("api/EmisionCombustion/" + emisionDB.Id);
            var emisionStr = response.Content.ReadAsStringAsync();
            var emisionRta = JsonConvert.DeserializeObject<EmisionCombustion>(emisionStr.Result);

            // Assert

            // Valida que el código de respuesta HTTP sea 200 (OK)
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            // Valida que los atributos corresponden a los de la BD
            Assert.Equal(emisionDB.Nombre, emisionRta.Nombre);
            Assert.Equal(emisionDB.Descripcion, emisionRta.Descripcion);
            Assert.Equal(emisionDB.Tag, emisionRta.Tag);
            Assert.Equal(emisionDB.HorasOperacion, emisionRta.HorasOperacion);
            Assert.Equal(emisionDB.EficienciaCombustion, emisionRta.EficienciaCombustion);
            Assert.Equal(emisionDB.SistemaId, emisionRta.SistemaId);
            Assert.Equal(emisionDB.TipoFuenteId, emisionRta.TipoFuenteId);
            Assert.Equal(emisionDB.FactorEmisionId, emisionRta.FactorEmisionId);
        }

        [Fact(DisplayName = "Consultar Emision Combustion No Existente")]
        public async Task ConsultarEmisionCombustionNoExistente()
        {
            // Arrange
            var client = _fixture.Client;
            // Act
            var response = await client.GetAsync("api/EmisionCombustion/10000");
            // Assert
            var textoRta = response.Content.ReadAsStringAsync().Result;

            // Valida que el código de respuesta es 404 (NotFound)
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            // Valida que el mensaje de respuesta contenga "no encontrada"
            Assert.Contains("no encontrada", textoRta);
        }

        [Fact(DisplayName = "Consultar Conjunto de Emisiones Combustion")]
        public async Task ConsultaConjuntoEmisionesCombustion()
        {
            // Arrange
            var client = _fixture.Client;
            var sistemaDB = _fixture.GetRandomSistema();

            // Act
            var response = await client.GetAsync("api/EmisionCombustion/GetSet/" + sistemaDB.Id);
            var emisionesDB = _fixture.GetEmisionCombustionPorSistema(sistemaDB.Id);
            var content = await response.Content.ReadAsStringAsync();
            var emisonesRta = JsonConvert.DeserializeObject<List<EmisionCombustion>>(content);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(emisionesDB.Count(), emisonesRta.Count());
        }

        private EmisionCombustion CrearEmisionCombustionDTO(int id=1, int sistemaId=1)
        {
            return new EmisionCombustion
            {
                Id = id,
                Consecutivo = "12",
                Tag = "234",
                Nombre = "Emision Combustion Alfa",
                Descripcion = "Emision Combustion Alfa - Planta 1",
                HorasOperacion = 2340,
                EficienciaCombustion = 2.1m,
                Observacion = "Ninguna",
                SistemaId = sistemaId,
                TipoFuenteId = 1,
                FactorEmisionId = 1
            };
        } 

    }
}
