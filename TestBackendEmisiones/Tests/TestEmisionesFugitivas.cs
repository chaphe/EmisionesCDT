using BackendEmisiones.Models;
using Newtonsoft.Json;
using System.Net;
using TestBackendEmisiones.Fixture;
using TestBackendEmisiones.Helper;

namespace TestBackendEmisiones.Tests
{
    [Collection("Backend EmisionCDT Collection")]
    public class TestEmisionesFugitivas
    {
        private readonly WebApplicationFactoryFixture _fixture;

        public TestEmisionesFugitivas(WebApplicationFactoryFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = "Crear Emision Fugitiva OK")]
        public async Task CrearEmisionFugutivaOK()
        {
            // Arrange
            var client = _fixture.Client;
            var emision = CrearEmisionFugitivaDTO();
            var content = HttpHelper.GetJsonHttpContent(emision);

            // Act
            var response = await client.PostAsync("api/EmisionFugitiva", content);
            var emisionStr = response.Content.ReadAsStringAsync();
            var emisionRta = JsonConvert.DeserializeObject<EmisionFugitiva>(emisionStr.Result);
            var emisionDB = _fixture.GetEmisionFugitivaPorId(emisionRta.Id);
            // Assert

            // Valida que el çodigo HTTP de respuesta es 200 (OK)
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            // Valida que los atributos en la BD son iguales al DTO
            Assert.Equal(emisionDB.Nombre, emisionRta.Nombre);
            Assert.Equal(emisionDB.Descripcion, emisionRta.Descripcion);
            Assert.Equal(emisionDB.Tag, emisionRta.Tag);
            Assert.Equal(emisionDB.HorasOperacion, emisionRta.HorasOperacion);
            Assert.Equal(emisionDB.FechaDeteccion, emisionRta.FechaDeteccion);
            Assert.Equal(emisionDB.FechaReparacion, emisionRta.FechaReparacion);
            Assert.Equal(emisionDB.SistemaId, emisionRta.SistemaId);
            Assert.Equal(emisionDB.TipoFuenteId, emisionRta.TipoFuenteId);
            Assert.Equal(emisionDB.FactorEmisionId, emisionRta.FactorEmisionId);
        }

        [Fact(DisplayName = "Crear Emision Fugitiva Sistema No Existente")]
        public async Task CrearEmisionFugitivaSistemaNoExistente()
        {
            // Arrange
            var client = _fixture.Client;
            var emision = CrearEmisionFugitivaDTO(1, 10000);

            // Act
            var content = HttpHelper.GetJsonHttpContent(emision);
            var response = await client.PostAsync("api/EmisionFugitiva", content);
            var textoStr = response.Content.ReadAsStringAsync().Result;


            // Assert
            // Valida que el çodigo HTTP de respuesta es 404 (Not Found)
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            //Valida que el mensaje regresado contenga "No existe"
            Assert.Contains("No existe", textoStr);
        }

        [Fact(DisplayName = "Actualizar Emision Fugitiva OK")]
        public async Task ActualizarEmisionFugitivaOK()
        {
            // Arrange
            var client = _fixture.Client;
            var emisionDB = _fixture.GetRandomEmisionFugitiva();
            var emision = CrearEmisionFugitivaDTO(emisionDB.Id, emisionDB.SistemaId);

            // Act
            var content = HttpHelper.GetJsonHttpContent(emision);
            var response = await client.PutAsync("api/EmisionFugitiva", content);
            var emisionStr = response.Content.ReadAsStringAsync();
            var emisionRta = JsonConvert.DeserializeObject<EmisionFugitiva>(emisionStr.Result);
            emisionDB = _fixture.GetEmisionFugitivaPorId(emision.Id);

            // Assert
            // Valida que la respuesta HTTP es 200 (OK)
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            // Valida que los atributos fueron actualizados correctamente
            // Valida que los atributos en la BD son iguales al DTO
            Assert.Equal(emisionDB.Nombre, emisionRta.Nombre);
            Assert.Equal(emisionDB.Descripcion, emisionRta.Descripcion);
            Assert.Equal(emisionDB.Tag, emisionRta.Tag);
            Assert.Equal(emisionDB.HorasOperacion, emisionRta.HorasOperacion);
            Assert.Equal(emisionDB.FechaDeteccion, emisionRta.FechaDeteccion);
            Assert.Equal(emisionDB.FechaReparacion, emisionRta.FechaReparacion);
            Assert.Equal(emisionDB.SistemaId, emisionRta.SistemaId);
            Assert.Equal(emisionDB.TipoFuenteId, emisionRta.TipoFuenteId);
            Assert.Equal(emisionDB.FactorEmisionId, emisionRta.FactorEmisionId);
        }

        [Fact(DisplayName = "Actualizar Emision Fugitiva No Existente")]
        public async Task ActualizarEmisionFugitivaNoExistente()
        {
            // Arrange
            var client = _fixture.Client;
            var emision = CrearEmisionFugitivaDTO(10000);

            // Act
            var content = HttpHelper.GetJsonHttpContent(emision);
            var response = await client.PutAsync("api/EmisionFugitiva", content);
            var textoRta = response.Content.ReadAsStringAsync().Result;

            // Assert
            // Valida que el çodigo HTTP de respuesta es 404 (Not Found)
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            // Valida que el mensaje de error contiene "no encontrada"
            Assert.Contains("no encontrada", textoRta);
        }

        [Fact(DisplayName = "Consultar Emision Fugitiva OK")]
        public async Task ConsultarEmisionFugitivaOK()
        {
            // Arrange
            var client = _fixture.Client;
            var emisionDB = _fixture.GetRandomEmisionFugitiva();
            // Act
            var response = await client.GetAsync("api/EmisionFugitiva/" + emisionDB.Id);
            var emisionStr = response.Content.ReadAsStringAsync();
            var emisionRta = JsonConvert.DeserializeObject<EmisionFugitiva>(emisionStr.Result);

            // Assert

            // Valida que el código de respuesta HTTP sea 200 (OK)
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            // Valida que los atributos en la BD son iguales al DTO
            Assert.Equal(emisionDB.Nombre, emisionRta.Nombre);
            Assert.Equal(emisionDB.Descripcion, emisionRta.Descripcion);
            Assert.Equal(emisionDB.Tag, emisionRta.Tag);
            Assert.Equal(emisionDB.HorasOperacion, emisionRta.HorasOperacion);
            Assert.Equal(emisionDB.FechaDeteccion, emisionRta.FechaDeteccion);
            Assert.Equal(emisionDB.FechaReparacion, emisionRta.FechaReparacion);
            Assert.Equal(emisionDB.SistemaId, emisionRta.SistemaId);
            Assert.Equal(emisionDB.TipoFuenteId, emisionRta.TipoFuenteId);
            Assert.Equal(emisionDB.FactorEmisionId, emisionRta.FactorEmisionId);
        }

        [Fact(DisplayName = "Consultar Emision Fugitiva No Existente")]
        public async Task ConsultarEmisionCombustionNoExistente()
        {
            // Arrange
            var client = _fixture.Client;
            // Act
            var response = await client.GetAsync("api/EmisionFugitiva/10000");
            // Assert
            var textoRta = response.Content.ReadAsStringAsync().Result;

            // Valida que el código de respuesta es 404 (NotFound)
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            // Valida que el mensaje de respuesta contenga "no encontrada"
            Assert.Contains("no encontrada", textoRta);
        }

        [Fact(DisplayName = "Consultar Conjunto de Emisiones Fugitivas")]
        public async Task ConsultaConjuntoEmisionesCombustion()
        {
            // Arrange
            var client = _fixture.Client;
            var sistemaDB = _fixture.GetRandomSistema();

            // Act
            var response = await client.GetAsync("api/EmisionFugitiva/GetSet/" + sistemaDB.Id);
            var emisionesDB = _fixture.GetEmisionFugitivaPorSistema(sistemaDB.Id);
            var content = await response.Content.ReadAsStringAsync();
            var emisonesRta = JsonConvert.DeserializeObject<List<EmisionFugitiva>>(content);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(emisionesDB.Count(), emisonesRta.Count());
        }

        private EmisionFugitiva CrearEmisionFugitivaDTO(int id = 1, int sistemaId = 1)
        {
            return new EmisionFugitiva
            {
                Id = id,
                Consecutivo = "12",
                Tag = "234",
                Nombre = "Emision Fugitiva Alfa",
                Descripcion = "Emision Fugitiva Alfa - Planta 1",
                HorasOperacion = 2340,
                Observacion = "Ninguna",
                SistemaId = sistemaId,
                Tamano = "10ft",
                Presion = 23,
                Temperatura = 124,             
                CaudalEmision = 23.2m,
                FactorGwp = 22,
                FechaDeteccion = new DateTime(2022, 5, 23),
                FechaReparacion = new DateTime(2022, 5, 23),
                Fuga = false,
                TipoFuenteId = 1,
                FactorEmisionId = 1
            };
        }
    }
}
