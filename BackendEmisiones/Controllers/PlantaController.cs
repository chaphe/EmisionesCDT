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
    public class PlantaController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;


        public PlantaController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("GetSet/{EmpresaId}")]
        public ActionResult<List<Planta>> GetSetPlantas(int EmpresaId)
        {
            var empresa = _context.Empresas.Find(EmpresaId); 
            if (empresa is null)
                return NotFound("No existe la empresa cosultada");
            
            var plantas = _context.Plantas
                .Where(p => p.EmpresaId == EmpresaId).ToList();
            return Ok(plantas);
        }


        [HttpPost]
        public ActionResult<Planta> AddPlanta(AddPlantaDto newPlanta)
        {
            //_context.Empresas.Include(e => e.Plantas);
            var empresa = _context.Empresas.Include(e => e.Plantas).FirstOrDefault(e => e.Id == newPlanta.EmpresaId);
            if (empresa is null)
            {
                return NotFound("No existe la empresa definida");
            }
            var planta = _mapper.Map<Planta>(newPlanta);
            planta.Empresa = empresa;
            empresa.Plantas.Add(planta);
            _context.SaveChanges();
            return Ok(planta);
        }

        [HttpGet("{id}")]
        public ActionResult<Planta> GetPlantaById(int id)
        {
            var planta = _context.Plantas.Find(id);
            if (planta is null)
            {
                return NotFound("Planta no encontrada");
            }
            return Ok(planta);
        }

        [HttpPut]
        public ActionResult<Planta> UpdatePlanta(Planta updatePlanta)
        {
            var planta = _context.Plantas.Include(p => p.Empresa).FirstOrDefault(p => p.Id == updatePlanta.Id);

            if (planta is null)
            {
                return NotFound("Planta no encontrada");
            }

            planta.Ciudad = updatePlanta.Ciudad;
            planta.Nombre = updatePlanta.Nombre;

            _context.SaveChanges();
            return Ok(planta);
        }
    }
}
