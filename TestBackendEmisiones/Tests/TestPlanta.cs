using BackendEmisiones.Data;
using BackendEmisiones.Models;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TestEmisionesCDT.Fixture;
using TestEmisionesCDT.Helper;

namespace TestEmisionesCDT.Tests
{
    [Collection("EmisionCDT Collection")]
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
            var planta = new Planta //DTO que se envia como JSON
            {
                Nombre = "Planta de Acacias",
                Ciudad = "Acacias",
                EmpresaId = 1
            };
            var content = HttpHelper.GetJsonHttpContent(planta);

            // Act
            var response = await client.PostAsync("api/Planta", content);
            var plantaStr = response.Content.ReadAsStringAsync();
            var plantaRta = JsonConvert.DeserializeObject<Planta>(plantaStr.Result);

            // Assert

            // Evalua que el çodigo HTTP de respuesta es 200 (OK)
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            using (var scope = _fixture.Factory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DataContext>();
                var plantaDB = context.Plantas.Find(plantaRta.Id);

                // Evalua que los atribitos en la BD son igu
                Assert.Equal(plantaDB.Nombre, plantaRta.Nombre);
                Assert.Equal(plantaDB.Ciudad, plantaRta.Ciudad);
                Assert.Equal(plantaDB.EmpresaId, plantaRta.EmpresaId);
            }
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
            var response = await client.PostAsync("api/Planta", HttpHelper.GetJsonHttpContent(planta));
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
            Planta plantaDB = null;
            using (var scope = _fixture.Factory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DataContext>();
                plantaDB = context.Plantas.OrderBy(p => p.Id).Last();
            }


            var planta = new Planta //DTO que se envia como JSON
            {
                Id = plantaDB.Id,
                Nombre = "Planta de Acacias",
                Ciudad = "Barranquilla",
                EmpresaId = plantaDB.EmpresaId
            };

            // Act
            var response = await client.PutAsync("api/Planta", HttpHelper.GetJsonHttpContent(planta));
            var plantaStr = response.Content.ReadAsStringAsync();
            var plantaRta = JsonConvert.DeserializeObject<Planta>(plantaStr.Result);


            using (var scope = _fixture.Factory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DataContext>();
                plantaDB = context.Plantas.Find(planta.Id);
            }
            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(plantaDB.Nombre, plantaRta.Nombre);
            Assert.Equal(plantaDB.Ciudad, plantaRta.Ciudad);
            Assert.Equal("Barranquilla", plantaRta.Ciudad);
        }

        [Fact(DisplayName = "Actualizar Planta No Existente")]
        public async Task ActualizarPlantaNoExistente()
        {
            // Arrange
            var client = _fixture.Client;
            Planta plantaDB = null;

            var planta = new Planta //DTO que se envia como JSON
            {
                Id = 10000,
                Nombre = "Planta de Acacias",
                Ciudad = "Barranquilla",
                EmpresaId = 1
            };

            // Act
            var response = await client.PutAsync("api/Planta", HttpHelper.GetJsonHttpContent(planta));
            var plantaStr = response.Content.ReadAsStringAsync().Result;

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Contains("no encontrada", plantaStr);

        }

        [Fact(DisplayName = "Consultar Planta OK")]
        public async Task ConsultarPlantaOK()
        {
            // Arrange
            var client = _fixture.Client;
            // Act
            var response = await client.GetAsync("api/Planta/1");
            // Assert
            var plantaStr = response.Content.ReadAsStringAsync();
            var plantaRta = JsonConvert.DeserializeObject<Planta>(plantaStr.Result);

            // Evalua que el código de respuesta sea 200 (OK)
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            using (var scope = _fixture.Factory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DataContext>();
                var plantaDB = context.Plantas.Find(1);
                // Evalua que los atributos corresponden a los de la BD
                Assert.Equal(plantaDB.Nombre, plantaRta.Nombre);
                Assert.Equal(plantaDB.Ciudad, plantaRta.Ciudad);
                Assert.Equal(plantaDB.Id, plantaRta.Id);
            }
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

            // Evalua que el código de respuesta sea 404 (No encontrado)
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Contains("no encontrada", textoRta);
        }

        [Fact(DisplayName = "Consultar Conjunto de Plantas")]
        public async Task ConsultaConjuntoPlantas()
        {
            // Arrange
            var client = _fixture.Client;
            var idEmpresa = 1;
            using (var scope = _fixture.Factory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DataContext>();
                Random random = new Random();
                idEmpresa = random.Next(1, _fixture.MaxIdEmpresa(context.Empresas) + 1); 
            }

            // Act
            var response = await client.GetAsync("api/Planta/GetSet/" + idEmpresa);

            // Assert

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            var plantasRta = JsonConvert.DeserializeObject<List<Planta>>(content);

            using (var scope = _fixture.Factory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DataContext>();
                var plantasDB = context.Plantas.Where(p=> p.EmpresaId==idEmpresa).ToList();

                // Evalua que el numero de empresas es igual al de la BD
                Assert.Equal(plantasDB.Count(), plantasRta.Count());
                //Assert.Equal(plantasDB, plantasRta);
            }
        }


    }
}
