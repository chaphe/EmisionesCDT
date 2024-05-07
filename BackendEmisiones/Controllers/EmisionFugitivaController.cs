using AutoMapper;
using BackendEmisiones.Data;
using BackendEmisiones.Dtos;
using BackendEmisiones.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BackendEmisiones.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmisionFugitivaController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;


        public EmisionFugitivaController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("GetSet/{SistemaId}")]
        public ActionResult<List<EmisionFugitiva>> GetSetEmisiones(int SistemaId)
        {
            var emisiones = _context.EmisionesFugitivas.Where(ef => ef.SistemaId == SistemaId).ToList();
            if (emisiones.IsNullOrEmpty())
                return NotFound("No se encontraron emisiones para ese Sistema");

            return Ok(emisiones);
        }


        [HttpPost]
        public ActionResult<EmisionFugitiva> AddEmisionFugitiva(AddEmisionFugitivaDto newEmision)
        {
            //_context.Empresas.Include(e => e.Plantas);
            var sistema = _context.Sistemas.Include(s => s.EmisionesFugitivas).FirstOrDefault(e => e.Id == newEmision.SistemaId);
            if (sistema is null)
            {
                return NotFound("No existe el sistema asociado");
            }
            var emision = _mapper.Map<EmisionFugitiva>(newEmision);
            emision.Sistema = sistema;
            sistema.EmisionesFugitivas.Add(emision);
            _context.SaveChanges();
            return Ok(emision);

        }

        [HttpGet("{id}")]
        public ActionResult<EmisionFugitiva> GetEmisionFugitivaById(int id)
        {
            var emision = _context.EmisionesFugitivas.Find(id);
            if (emision is null)
            {
                return NotFound("Emision no encontrada");
            }
            return Ok(emision);
        }

        [HttpPut]
        public ActionResult<EmisionFugitiva> UpdateEmisionFugitiva(EmisionFugitiva updatedEmision)
        {
            var emision = _context.EmisionesFugitivas.Include(ef => ef.Sistema).FirstOrDefault(ef => ef.Id == updatedEmision.Id);

            if (emision is null)
            {
                return NotFound("Emision no encontrada");
            }

            emision.CaudalEmision = updatedEmision.CaudalEmision;
            emision.FactorEmisionId = updatedEmision.FactorEmisionId;
            emision.FechaDeteccion = updatedEmision.FechaDeteccion;
            emision.FechaReparacion = updatedEmision.FechaReparacion;
            emision.Observacion = updatedEmision.Observacion;
            emision.Descripcion = updatedEmision.Descripcion;
            emision.HorasOperacion = updatedEmision.HorasOperacion;

            _context.SaveChanges();
            return Ok(emision);
        }
    }
}
