using BackendEmisiones.Dtos;
using BackendEmisiones.Models;
using Newtonsoft.Json;
using System.Net;
using TestBackendEmisiones.Fixture;
using TestBackendEmisiones.Helper;

namespace TestBackendEmisiones.Tests
{
    [Collection("Backend EmisionCDT Collection")]
    public class TestEvidencia
    {
        private readonly WebApplicationFactoryFixture _fixture;

        public TestEvidencia(WebApplicationFactoryFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = "Crear Evidencia OK")]
        public async Task CrearEvidenciaOK()
        {
            // Arrange
            var client = _fixture.Client;
            var evidencia = CrearEvidenciaDTO();        
            var content = HttpHelper.GetJsonHttpContent(evidencia);

            // Act
            var response = await client.PostAsync("api/Evidencia", content);
            var evidenciaStr = response.Content.ReadAsStringAsync();
            var evidenciaRta = JsonConvert.DeserializeObject<Evidencia>(evidenciaStr.Result);
            var evidenciaDB = _fixture.GetEvidenciaPorId(evidenciaRta.Id);
            // Assert

            // Valida que el çodigo HTTP de respuesta es 200 (OK)
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            // Valida que los atributos en la BD son iguales al DTO
            Assert.Equal(evidenciaDB.UsuarioDeteccionId, evidenciaRta.UsuarioDeteccionId);
            Assert.Equal(evidenciaDB.UsuarioReparacionId, evidenciaRta.UsuarioReparacionId);       
            Assert.Equal(evidenciaDB.FechaDeteccion, evidenciaRta.FechaDeteccion);
            Assert.Equal(evidenciaDB.FechaReparacion, evidenciaRta.FechaReparacion);
            Assert.Equal(evidenciaDB.FotoAntes, evidenciaRta.FotoAntes);
            Assert.Equal(evidenciaDB.FotoDespues, evidenciaRta.FotoDespues);
            Assert.Equal(evidenciaDB.Video, evidenciaRta.Video);
            Assert.Equal(evidenciaDB.EmisionFugitivaId, evidenciaRta.EmisionFugitivaId);
        }

        [Fact(DisplayName = "Crear Evidencia Emision No Existente")]
        public async Task CrearEvidenciaEmisionNoExistente()
        {
            // Arrange
            var client = _fixture.Client;
            var evidencia = CrearEvidenciaDTO(1, 10000);

            // Act
            var content = HttpHelper.GetJsonHttpContent(evidencia);
            var response = await client.PostAsync("api/Evidencia", content);
            var textoStr = response.Content.ReadAsStringAsync().Result;


            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Contains("No existe", textoStr);
        }

        [Fact(DisplayName = "Actualizar Evidencia OK")]
        public async Task ActualizarEvidenciaOK()
        {
            // Arrange
            var client = _fixture.Client;
            var evidenciaDB = _fixture.GetRandomEvidencia();
            var evidencia = CrearEvidenciaDTO(evidenciaDB.Id, evidenciaDB.EmisionFugitivaId);

            // Act
            var content = HttpHelper.GetJsonHttpContent(evidencia);
            var response = await client.PutAsync("api/Evidencia", content);
            var evidenciaStr = response.Content.ReadAsStringAsync();
            var evidenciaRta = JsonConvert.DeserializeObject<Evidencia>(evidenciaStr.Result);
            evidenciaDB = _fixture.GetEvidenciaPorId(evidencia.Id);

            // Assert
            // Valida que la respuesta HTTP es 200 (OK)
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            // Valida que los atributos fueron actualizados correctamente
            Assert.Equal(evidenciaDB.UsuarioDeteccionId, evidenciaRta.UsuarioDeteccionId);
            Assert.Equal(evidenciaDB.UsuarioReparacionId, evidenciaRta.UsuarioReparacionId);
            Assert.Equal(evidenciaDB.FechaDeteccion, evidenciaRta.FechaDeteccion);
            Assert.Equal(evidenciaDB.FechaReparacion, evidenciaRta.FechaReparacion);
            Assert.Equal(evidenciaDB.FotoAntes, evidenciaRta.FotoAntes);
            Assert.Equal(evidenciaDB.FotoDespues, evidenciaRta.FotoDespues);
            Assert.Equal(evidenciaDB.Video, evidenciaRta.Video);
            Assert.Equal(evidenciaDB.EmisionFugitivaId, evidenciaRta.EmisionFugitivaId);
        }

        [Fact(DisplayName = "Actualizar Evidencia No Existente")]
        public async Task ActualizarEvidenciaNoExistente()
        {
            // Arrange
            var client = _fixture.Client;
            var evidencia = CrearEvidenciaDTO(10000);

            // Act
            var content = HttpHelper.GetJsonHttpContent(evidencia);
            var response = await client.PutAsync("api/Evidencia", content);
            var textoRta = response.Content.ReadAsStringAsync().Result;

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Contains("no encontrada", textoRta);
        }

        [Fact(DisplayName = "Consultar Evidencia OK")]
        public async Task ConsultarEvidenciaOK()
        {
            // Arrange
            var client = _fixture.Client;
            var evidenciaDB = _fixture.GetRandomEvidencia();
            // Act
            var response = await client.GetAsync("api/Evidencia/" + evidenciaDB.Id);
            var evidenciaStr = response.Content.ReadAsStringAsync();
            var evidenciaRta = JsonConvert.DeserializeObject<Evidencia>(evidenciaStr.Result);

            // Assert

            // Valida que el código de respuesta HTTP sea 200 (OK)
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            // Valida que los atributos corresponden a los de la BD
            Assert.Equal(evidenciaDB.UsuarioDeteccionId, evidenciaRta.UsuarioDeteccionId);
            Assert.Equal(evidenciaDB.UsuarioReparacionId, evidenciaRta.UsuarioReparacionId);
            Assert.Equal(evidenciaDB.FechaDeteccion, evidenciaRta.FechaDeteccion);
            Assert.Equal(evidenciaDB.FechaReparacion, evidenciaRta.FechaReparacion);
            Assert.Equal(evidenciaDB.FotoAntes, evidenciaRta.FotoAntes);
            Assert.Equal(evidenciaDB.FotoDespues, evidenciaRta.FotoDespues);
            Assert.Equal(evidenciaDB.Video, evidenciaRta.Video);
            Assert.Equal(evidenciaDB.EmisionFugitivaId, evidenciaRta.EmisionFugitivaId);
        }

        [Fact(DisplayName = "Consultar Evidencia No Existente")]
        public async Task ConsultarSistemaNoExistente()
        {
            // Arrange
            var client = _fixture.Client;
            // Act
            var response = await client.GetAsync("api/Evidencia/10000");
            // Assert
            var textoRta = response.Content.ReadAsStringAsync().Result;

            // Evalua que el código de respuesta sea 404 (No encontrado)
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Contains("no encontrada", textoRta);
        }

        [Fact(DisplayName = "Consultar Conjunto de Evidencias")]
        public async Task ConsultarConjuntoEvidencias()
        {
            // Arrange
            var client = _fixture.Client;
            var emisionDB = _fixture.GetRandomEmisionFugitiva();

            // Act
            var response = await client.GetAsync("api/Evidencia/GetSet/" + emisionDB.Id);
            var evidenciasDB = _fixture.GetEvidenciasPorEmision(emisionDB.Id);
            var content = await response.Content.ReadAsStringAsync();
            var sistemasRta = JsonConvert.DeserializeObject<List<Evidencia>>(content);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(evidenciasDB.Count(), sistemasRta.Count());
        }

        private Sistema CrearSistemaDTO(int id=1, int plantaId=1)
        {
            return new Sistema
            {
                Id = id,
                Nombre = "Almacenamiento GLP",
                Descripcion = "Amnto GLP - Acacias 1",
                PlantaId = plantaId
            };
        }

        private Evidencia CrearEvidenciaDTO(int id = 1, int emisionId = 1)
        {
            return new Evidencia
            {
                Id = id,
                UsuarioDeteccionId = 1,
                UsuarioReparacionId = 1,
                FotoAntes = false,
                FotoDespues = false,
                Video = false,
                FechaDeteccion = new DateTime(2022, 5, 23),
                FechaReparacion = new DateTime(2022, 10, 23),
                EmisionFugitivaId = emisionId
            };
        }
    }
}
