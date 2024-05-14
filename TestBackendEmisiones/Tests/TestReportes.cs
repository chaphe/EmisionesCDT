using BackendEmisiones.Dtos;
using BackendEmisiones.Models;
using Newtonsoft.Json;
using System.Net;
using TestBackendEmisiones.Fixture;
using TestBackendEmisiones.Helper;

namespace TestBackendEmisiones.Tests
{
    [Collection("Backend EmisionCDT Collection")]
    public class TestReportes
    {
        private readonly WebApplicationFactoryFixture _fixture;

        public TestReportes(WebApplicationFactoryFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = "Generar Reporte Mensual OK")]
        public async Task GenerarReporteMensualOK()
        {
            // Arrange
            var client = _fixture.Client;
            var planta = _fixture.GetRandomPlanta();
            var reporte = CrearReporteMensualDTO(planta.Id);
            var content = HttpHelper.GetJsonHttpContent(reporte);

            // Act
            var response = await client.PostAsync("api/Reportes/Mensual", content);
            var reporteStr = response.Content.ReadAsStringAsync();
            var reporteRta = JsonConvert.DeserializeObject<ReporteMensual>(reporteStr.Result);
            //var sistemaDB = _fixture.GetSistemaPorId(reporteRta.Id);
            // Assert

            // Valida que el çodigo HTTP de respuesta es 200 (OK)
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            // Valida que los atributos en la BD son iguales al DTO
            Assert.Equal(reporte.PlantaID, reporteRta.PlantaId);
            Assert.Equal(reporte.Mes, reporteRta.Mes);
            Assert.Equal(reporte.Anho, reporteRta.Anho);

        }

        [Fact(DisplayName = "Consultar Reporte Mensual No Existente")]
        public async Task ConsultarReporteMensualNoExistente()
        {
            // Arrange
            var client = _fixture.Client;


            // Act
            var response = await client.GetAsync("api/Reportes/Mensual/10000");
            var textoRta = response.Content.ReadAsStringAsync().Result;


            // Assert
            // Valida que el código de respuesta sea 404 (No encontrado)
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Contains("no encontrado", textoRta);
        }

        [Fact(DisplayName = "Consultar Reporte Mensual Gas No Existente")]
        public async Task ConsultarReporteMensualGasNoExistente()
        {
            // Arrange
            var client = _fixture.Client;


            // Act
            var response = await client.GetAsync("api/Reportes/MensualGas/10000");
            var textoRta = response.Content.ReadAsStringAsync().Result;


            // Assert
            // Valida que el código de respuesta sea 404 (No encontrado)
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Contains("no encontrado", textoRta);
        }


        [Fact(DisplayName = "Generar Reportar Mensual Planta No Existente")]
        public async Task GenerarReporteMensualPlantaNoExistente()
        {
            // Arrange
            var client = _fixture.Client;
            var reporte = CrearReporteMensualDTO(10000);
            var content = HttpHelper.GetJsonHttpContent(reporte);


            // Act
            var response = await client.PostAsync("api/Reportes/Mensual", content);
            var textoStr = response.Content.ReadAsStringAsync().Result;


            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Contains("No existe", textoStr);
        }

        [Fact(DisplayName = "Generar Reporte Mensual Gas OK")]
        public async Task GenerarReporteMensualGasOK()
        {
            // Arrange
            var client = _fixture.Client;
            var planta = _fixture.GetRandomPlanta();
            var reporte = CrearReporteMensualGasDTO(planta.Id, 1);
            var content = HttpHelper.GetJsonHttpContent(reporte);

            // Act
            var response = await client.PostAsync("api/Reportes/MensualGas", content);
            var reporteStr = response.Content.ReadAsStringAsync();
            var reporteRta = JsonConvert.DeserializeObject<ReporteMensualGas>(reporteStr.Result);

            // Assert
            // Valida que la respuesta HTTP es 200 (OK)
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            // Valida que los atributos en la BD son iguales al DTO
            Assert.Equal(reporte.PlantaID, reporteRta.PlantaId);
            Assert.Equal(reporte.Mes, reporteRta.Mes);
            Assert.Equal(reporte.Anho, reporteRta.Anho);
            Assert.Equal(reporte.GasId, reporteRta.GasId);
        }

        [Fact(DisplayName = "Generar Reportar Mensual Gas Planta No Existente")]
        public async Task GenerarReporteMensualGasPlantaNoExistente()
        {
            // Arrange
            var client = _fixture.Client;
            var reporte = CrearReporteMensualGasDTO(10000, 1);
            var content = HttpHelper.GetJsonHttpContent(reporte);


            // Act
            var response = await client.PostAsync("api/Reportes/MensualGas", content);
            var textoStr = response.Content.ReadAsStringAsync().Result;


            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Contains("No existe", textoStr);
        }


        [Fact(DisplayName = "Consultar Conjunto Reporte Mensual")]
        public async Task ConsultarConjuntoReporteMensual()
        {
            // Arrange
            var client = _fixture.Client;
            //var plantaDB = _fixture.GetRandomPlanta();

            // Act
            var response = await client.GetAsync("api/Reportes/Mensual/GetAll");
            //var sistemasDB = _fixture.GetSistemasPorPlanta(plantaDB.Id);
            var content = await response.Content.ReadAsStringAsync();
            var reportesRta = JsonConvert.DeserializeObject<List<ReporteMensual>>(content);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(2, reportesRta.Count);
        }

        [Fact(DisplayName = "Consultar Conjunto Reporte Mensual Gas")]
        public async Task ConsultarConjuntoReporteMensualGas()
        {
            // Arrange
            var client = _fixture.Client;
            //var plantaDB = _fixture.GetRandomPlanta();

            // Act
            var response = await client.GetAsync("api/Reportes/MensualGas/GetAll");
            //var sistemasDB = _fixture.GetSistemasPorPlanta(plantaDB.Id);
            var content = await response.Content.ReadAsStringAsync();
            var reportesRta = JsonConvert.DeserializeObject<List<ReporteMensualGas>>(content);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(2, reportesRta.Count);
        }




        private GenerarReporteMensualDto CrearReporteMensualDTO(int plantaId)
        {
            return new GenerarReporteMensualDto
            {
                PlantaID = plantaId,
                Mes = 6,
                Anho = 2023
            };
        }

        private GenerarReporteMensualGasDto CrearReporteMensualGasDTO(int plantaId, int gasId)
        {
            return new GenerarReporteMensualGasDto
            {
                PlantaID = plantaId,
                GasId = gasId,
                Mes = 6,
                Anho = 2023
            };
        }

    }
}
