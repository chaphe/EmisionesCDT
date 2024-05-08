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
    public class TestSistema
    {
        private readonly WebApplicationFactoryFixture _fixture;

        public TestSistema(WebApplicationFactoryFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = "Crear Sistema OK")]
        public async Task CrearSistemaOK()
        {
            // Arrange
            var client = _fixture.Client;
            var sistema = new Sistema //DTO que se envia como JSON
            {
                Nombre = "Almacenamiento GLP",
                Descripcion = "Amnto GLP - Acacias 1",
                PlantaId = 1
            };
            var content = HttpHelper.GetJsonHttpContent(sistema);

            // Act
            var response = await client.PostAsync("api/Sistema", content);
            var sistemaStr = response.Content.ReadAsStringAsync();
            var sistemaRta = JsonConvert.DeserializeObject<Sistema>(sistemaStr.Result);
            var sistemaDB = _fixture.GetSistemaPorId(sistemaRta.Id);
            // Assert

            // Valida que el çodigo HTTP de respuesta es 200 (OK)
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            // Valida que los atributos en la BD son iguales al DTO
            Assert.Equal(sistemaDB.Nombre, sistemaRta.Nombre);
            Assert.Equal(sistemaDB.Descripcion, sistemaRta.Descripcion);
            Assert.Equal(sistemaDB.PlantaId, sistemaRta.PlantaId);
        }

        [Fact(DisplayName = "Crear Sistema Planta No Existente")]
        public async Task CrearSistemaPlantaNoExistente()
        {
            // Arrange
            var client = _fixture.Client;
            var sistema = new Sistema //DTO que se envia como JSON
            {
                Nombre = "Almacenamiento GLP",
                Descripcion = "Amnto GLP - Acacias 1",
                PlantaId = 10000
            };

            // Act
            var content = HttpHelper.GetJsonHttpContent(sistema);
            var response = await client.PostAsync("api/Sistema", content);
            var textoStr = response.Content.ReadAsStringAsync().Result;


            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Contains("No existe", textoStr);
        }

        [Fact(DisplayName = "Actualizar Sistema OK")]
        public async Task ActualizarSistemaOK()
        {
            // Arrange
            var client = _fixture.Client;
            var sistemaDB = _fixture.GetRandomSistema();
            var sistema = new Sistema //DTO que se envia como JSON
            {
                Id = sistemaDB.Id,
                Nombre = "Almacenamiento GLP Nuevo",
                Descripcion = "Almacenamiento GLP - Acacias 1",
                PlantaId = sistemaDB.PlantaId
            };

            // Act
            var content = HttpHelper.GetJsonHttpContent(sistema);
            var response = await client.PutAsync("api/Sistema", content);
            var sistemaStr = response.Content.ReadAsStringAsync();
            var sistemaRta = JsonConvert.DeserializeObject<Sistema>(sistemaStr.Result);
            sistemaDB = _fixture.GetSistemaPorId(sistema.Id);

            // Assert
            // Valida que la respuesta HTTP es 200 (OK)
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            // Valida que los atributos fueron actualizados correctamente
            Assert.Equal(sistemaDB.Nombre, sistemaRta.Nombre);
            Assert.Equal(sistemaDB.Descripcion, sistemaRta.Descripcion);
            Assert.Equal(sistemaDB.PlantaId, sistemaRta.PlantaId);
            Assert.Equal(sistema.Nombre, sistemaRta.Nombre);
            Assert.Equal(sistema.Descripcion, sistemaRta.Descripcion);
        }

        [Fact(DisplayName = "Actualizar Sistema No Existente")]
        public async Task ActualizarSistemaNoExistente()
        {
            // Arrange
            var client = _fixture.Client;          

            var sistema = new Sistema //DTO que se envia como JSON
            {
                Id = 10000,
                Nombre = "Almacenamiento GLP Nuevo",
                Descripcion = "Almacenamiento GLP - Acacias 1",
                PlantaId = 1
            };

            // Act
            var content = HttpHelper.GetJsonHttpContent(sistema);
            var response = await client.PutAsync("api/Sistema", content);
            var sistemaStr = response.Content.ReadAsStringAsync().Result;

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Contains("no encontrado", sistemaStr);
        }

        [Fact(DisplayName = "Consultar Sistema OK")]
        public async Task ConsultarSistemaOK()
        {
            // Arrange
            var client = _fixture.Client;
            var sistemaDB = _fixture.GetRandomSistema();
            // Act
            var response = await client.GetAsync("api/Sistema/" + sistemaDB.Id);
            var sistemaStr = response.Content.ReadAsStringAsync();
            var sistemaRta = JsonConvert.DeserializeObject<Sistema>(sistemaStr.Result);

            // Assert

            // Valida que el código de respuesta HTTP sea 200 (OK)
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            // Valida que los atributos corresponden a los de la BD
            Assert.Equal(sistemaDB.Nombre, sistemaRta.Nombre);
            Assert.Equal(sistemaDB.Descripcion, sistemaRta.Descripcion);
            Assert.Equal(sistemaDB.Id, sistemaRta.Id);
            Assert.Equal(sistemaDB.PlantaId, sistemaRta.PlantaId);
        }

        [Fact(DisplayName = "Consultar Sistema No Existente")]
        public async Task ConsultarSistemaNoExistente()
        {
            // Arrange
            var client = _fixture.Client;
            // Act
            var response = await client.GetAsync("api/Sistema/10000");
            // Assert
            var textoRta = response.Content.ReadAsStringAsync().Result;

            // Evalua que el código de respuesta sea 404 (No encontrado)
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Contains("no encontrado", textoRta);
        }

        [Fact(DisplayName = "Consultar Conjunto de Sistemas")]
        public async Task ConsultaConjuntoPlantas()
        {
            // Arrange
            var client = _fixture.Client;
            var plantaDB = _fixture.GetRandomPlanta();


            // Act
            var response = await client.GetAsync("api/Sistema/GetSet/" + plantaDB.Id);
            var sistemasDB = _fixture.GetSistemasPorPlanta(plantaDB.Id);
            var content = await response.Content.ReadAsStringAsync();
            var sistemasRta = JsonConvert.DeserializeObject<List<Sistema>>(content);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(sistemasDB.Count(), sistemasRta.Count());
        }


    }
}
