using AutoMapper;
using BackendEmisiones.Data;
using BackendEmisiones.Dtos;
using BackendEmisiones.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendEmisiones.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportesController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public ReportesController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        [HttpGet("Mensual/{id}")]
        public ActionResult<ReporteMensual> GetReporteMensualById(int id)
        {
            var reporte = _context.ReporteMensual.Include(rm => rm.Items).FirstOrDefault(rm => rm.Id == id);

            if (reporte is null)
            {
                return NotFound("Reporte no encontrado");
            }
            return Ok(reporte);
        }

        [HttpGet("MensualGas/{id}")]
        public ActionResult<ReporteMensualGas> GetReporteMensualGasById(int id)
        {
            var reporte = _context.ReporteMensualGas.Include(rm => rm.Items).FirstOrDefault(rm => rm.Id == id);

            if (reporte is null)
            {
                return NotFound("Reporte no encontrado");
            }
            return Ok(reporte);
        }

        [HttpGet("Mensual/GetAll")]
        public ActionResult<List<ReporteMensual>> GetMensualAllReportes()
        {
            var reportes = _context.ReporteMensual.ToList();
            return Ok(reportes);
        }

        [HttpGet("MensualGas/GetAll")]
        public ActionResult<List<ReporteMensual>> GetMensualGasAllReportes()
        {
            var reportes = _context.ReporteMensualGas.ToList();
            return Ok(reportes);
        }

        [HttpPost("Mensual")]
        public ActionResult<List<ReporteMensual>> AddReporteMensual(GenerarReporteMensualDto dto)
        {

            var fechaInicio = new DateTime(dto.Anho, dto.Mes, 1);
            var fechaFin = new DateTime(dto.Anho, dto.Mes, 1).AddMonths(1).AddSeconds(-1);


            var reporteTemp = from ef in _context.EmisionesFugitivas
                              where (ef.Sistema.PlantaId == dto.PlantaID
                              && ef.FechaDeteccion <= fechaInicio
                              && (ef.FechaReparacion > fechaFin || ef.FechaReparacion == null))
                              group ef by new { ef.SistemaId, ef.FactorEmisionId } into grp
                              select new
                              {
                                  SistemaId = grp.Single().SistemaId,
                                  FactorEmisionId = grp.Single().FactorEmisionId,
                                  FactorActividad = grp.Sum(ef => (ef.CaudalEmision * ef.HorasOperacion / 12))
                              };

            var reporte = reporteTemp.ToList();

            var planta = _context.Plantas.Find(dto.PlantaID);
            if (planta is null)
                return NotFound("Planta No existe");


            var empresa = _context.Empresas.Find(planta.EmpresaId);
            if (empresa is null)
                return NotFound("Empresa No existe");

            var reporteMensual = new ReporteMensual();
            reporteMensual.PlantaId = planta.Id;
            reporteMensual.Planta = planta.Nombre;
            reporteMensual.Empresa = empresa.RazonSocial;
            reporteMensual.EmpresaId = empresa.Id;
            reporteMensual.Mes = dto.Mes;
            reporteMensual.Anho = dto.Anho;
            reporteMensual.Items = new List<ReporteMensualDetalle>();

            foreach (var item in reporte)
            {
                var sistema = _context.Sistemas.Find(item.SistemaId);
                if (sistema is null)
                    return NotFound("Sistema No existe");
                var newItem = new ReporteMensualDetalle();
                var gas = _context.FactoresEmision.Find(item.FactorEmisionId);
                if (gas is null)
                    return NotFound("Gas Referenciado No Existe");
                newItem.NombreGas = gas.NombreGas;
                newItem.FactorEmisionId = gas.Id;
                newItem.Enision = item.FactorActividad;
                newItem.EmisionCO2 = item.FactorActividad * gas.ValorCo2fugitivas;
                newItem.Sistema = sistema.Nombre;
                newItem.SistemaId = sistema.Id;
                newItem.ReporteMensual = reporteMensual;
                reporteMensual.Items.Add(newItem);
            }
            _context.ReporteMensual.Add(reporteMensual);
            _context.SaveChanges();

            return Ok(reporteMensual);
        }

        [HttpPost("MensualGas")]
        public ActionResult<List<ReporteMensual>> AddReporteMensualGas(GenerarReporteMensualGasDto dto)
        {

            var fechaInicio = new DateTime(dto.Anho, dto.Mes, 1);
            var fechaFin = new DateTime(dto.Anho, dto.Mes, 1).AddMonths(1).AddSeconds(-1);



            var reporteTemp = from ef in _context.EmisionesFugitivas
                              where (ef.Sistema.PlantaId == dto.PlantaID
                              && ef.FactorEmisionId == dto.GasId
                              && ef.FechaDeteccion <= fechaInicio
                              && (ef.FechaReparacion > fechaFin || ef.FechaReparacion == null))
                              group ef by ef.SistemaId into grp
                              select new
                              {
                                  SistemaId = grp.Single().SistemaId,
                                  FactorActividad = grp.Sum(ef => (ef.CaudalEmision * ef.HorasOperacion / 12))
                              };

            var reporte = reporteTemp.ToList();

            var planta = _context.Plantas.Find(dto.PlantaID);
            if (planta is null)
                return NotFound("Planta No existe");


            var empresa = _context.Empresas.Find(planta.EmpresaId);
            if (empresa is null)
                return NotFound("Empresa No existe");

            var gas = _context.FactoresEmision.Find(dto.GasId);

            if (gas is null)
                return NotFound("Gas no existe");

            var reporteMensual = new ReporteMensualGas();
            reporteMensual.PlantaId = planta.Id;
            reporteMensual.Planta = planta.Nombre;
            reporteMensual.Empresa = empresa.RazonSocial;
            reporteMensual.EmpresaId = empresa.Id;
            reporteMensual.Mes = dto.Mes;
            reporteMensual.Anho = dto.Anho;
            reporteMensual.GasId = gas.Id;
            reporteMensual.Gas = gas.NombreGas;

            reporteMensual.Items = new List<ReporteMensualGasDetalle>();

            foreach (var item in reporte)
            {
                var sistema = _context.Sistemas.Find(item.SistemaId);
                if (sistema is null)
                    return NotFound("Sistema No existe");
                var newItem = new ReporteMensualGasDetalle();
                newItem.Enision = item.FactorActividad;
                newItem.EmisionCO2 = item.FactorActividad * gas.ValorCo2fugitivas;
                newItem.Sistema = sistema.Nombre;
                newItem.SistemaId = sistema.Id;
                newItem.ReporteMensual = reporteMensual;
                reporteMensual.Items.Add(newItem);
            }
            _context.ReporteMensualGas.Add(reporteMensual);
            _context.SaveChanges();

            return Ok(reporteMensual);
        }
    }
}
