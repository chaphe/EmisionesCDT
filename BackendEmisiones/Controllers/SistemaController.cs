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
    public class SistemaController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;


        public SistemaController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("GetSet/{PlantaId}")]
        public ActionResult<List<Planta>> GetSetSistemas(int PlantaId)
        {
            var sistemas = _context.Sistemas.Where(s => s.PlantaId == PlantaId).ToList();
            if (sistemas.IsNullOrEmpty())
                return NotFound("No se encontraron sistemas para esa planta");
            return Ok(sistemas);
        }


        [HttpPost]
        public ActionResult<Sistema> AddSistema(AddSistemaDto newSistema)
        {
            var planta = _context.Plantas.Include(p => p.Sistemas)
                .FirstOrDefault(p => p.Id == newSistema.PlantaId);
            if (planta is null)
                return NotFound("No existe la planta definida");
            var sistema = _mapper.Map<Sistema>(newSistema);
            sistema.Planta = planta;
            planta.Sistemas.Add(sistema);
            _context.SaveChanges();
            return Ok(sistema);
        }

        [HttpGet("{id}")]
        public ActionResult<Sistema> GetSistemaById(int id)
        {
            var sistema = _context.Sistemas.Find(id);
            if (sistema is null)
            {
                return NotFound("Sistema no encontrado");
            }
            return Ok(sistema);
        }

        [HttpPut]
        public ActionResult<Sistema> ActualizarSistema(Sistema updateSistema)
        {
            var sistema = _context.Sistemas.FirstOrDefault(s => s.Id == updateSistema.Id);

            if (sistema is null)

                return NotFound("Sistema no encontrado");

            sistema.Nombre = updateSistema.Nombre;
            sistema.Descripcion = updateSistema.Descripcion;

            _context.SaveChanges();
            return Ok(sistema);
        }
    }
}
