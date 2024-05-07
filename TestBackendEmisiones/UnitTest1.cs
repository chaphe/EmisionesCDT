using BackendEmisiones;
using BackendEmisiones.Controllers;
using BackendEmisiones.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Text.Json.Nodes;
using System.Text;
using Xunit.Abstractions;

namespace TestEmisionesCDT
{
    public class UnitTest1 : IClassFixture<WebApplicationFactory<Program>>
    {

        private readonly WebApplicationFactory<Program> _factory;

        public UnitTest1(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task AgregarEmpresaOK()
        {
            // Arrange
            var client = _factory.CreateClient();
            var empresa = new Empresa
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

            var content = new StringContent(JsonConvert.SerializeObject(empresa), Encoding.UTF8, "application/json");
         
            // Act
            var response = await client.PostAsync("api/Empresa", content);
            var empresaStr = response.Content.ReadAsStringAsync();
            var empresaRta = JsonConvert.DeserializeObject<Empresa>(empresaStr.Result);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(empresa.Identificacion, empresaRta.Identificacion);

        }

        [Fact]
        public async Task ConsultaConjuntoEmpresas()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("api/Empresa/GetAll");

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Console.WriteLine(response.StatusCode.ToString());
            //Assert.Equal(response.StatusCode.GetType())
            Assert.Equal("application/json; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            var empresas = JsonConvert.DeserializeObject<List<Empresa>>(content);

            Assert.NotEmpty(empresas);
        }
    }
}