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
    public class EmisionCombustionController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;


        public EmisionCombustionController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("GetSet/{SistemaId}")]
        public ActionResult<List<EmisionCombustion>> GetSetEmisiones(int SistemaId)
        {
            var sistema = _context.Sistemas.Find(SistemaId);
            if (sistema is null)
                return NotFound("No existe el sistema consultado");

            var emisiones = _context.EmisionesCombustion
                .Where(ec => ec.SistemaId == SistemaId).ToList();         
            return Ok(emisiones);
        }


        [HttpPost]
        public ActionResult<EmisionCombustion> AddEmisionCombustion(AddEmisionCombustionDto newEmision)
        {
            //_context.Empresas.Include(e => e.Plantas);
            var sistema = _context.Sistemas.Include(s => s.EmisionesCombustion).FirstOrDefault(e => e.Id == newEmision.SistemaId);
            if (sistema is null)
            {
                return NotFound("No existe el sistema asociado");
            }
            var emision = _mapper.Map<EmisionCombustion>(newEmision);
            emision.Sistema = sistema;
            sistema.EmisionesCombustion.Add(emision);
            _context.SaveChanges();
            return Ok(emision);
        }

        [HttpGet("{id}")]
        public ActionResult<EmisionCombustion> GetEmisionCombustionById(int id)
        {
            var emision = _context.EmisionesCombustion.Find(id);
            if (emision is null)
            {
                return NotFound("Emision no encontrada");
            }
            return Ok(emision);
        }

        [HttpPut]
        public ActionResult<EmisionCombustion> UpdateEmisionFugitiva(EmisionCombustion updatedEmision)
        {
            var emision = _context.EmisionesCombustion.Find(updatedEmision.Id);

            if (emision is null)
            {
                return NotFound("Emision no encontrada");
            }


            emision.Nombre = updatedEmision.Nombre;
            emision.Tag = updatedEmision.Tag;
            emision.Consecutivo = updatedEmision.Consecutivo;
            emision.Observacion = updatedEmision.Observacion;
            emision.Descripcion = updatedEmision.Descripcion;
            emision.HorasOperacion = updatedEmision.HorasOperacion;
            emision.EficienciaCombustion = updatedEmision.EficienciaCombustion;

            _context.SaveChanges();
            return Ok(emision);
        }
    }
}
