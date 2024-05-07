using AutoMapper;
using BackendEmisiones.Data;
using BackendEmisiones.Dtos;
using BackendEmisiones.Models;
using Microsoft.AspNetCore.Mvc;

namespace BackendEmisiones.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmpresaController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public EmpresaController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("GetAll")]
        public ActionResult<List<Empresa>> GetEmpresas()
        {
            return Ok(_context.Empresas.ToList());
        }

        [HttpGet("{id}")]
        public ActionResult<Empresa> GetEmpresaById(int id)
        {
            var empresa = _context.Empresas.Find(id);
            if (empresa is null)
            {
                return NotFound("Empresa no encontrada");
            }
            return Ok(empresa);
        }

        [HttpPost]
        public ActionResult<Empresa> AddEmpresa(AddEmpresaDto newEmpresa)
        {
            var empresa = _mapper.Map<Empresa>(newEmpresa);
            _context.Empresas.Add(empresa);
            _context.SaveChanges();
            return Ok(empresa);
        }

        [HttpPut]
        public ActionResult<Empresa> UpdateEmpresa(Empresa updateEmpresa)
        {
            var empresa = _context.Empresas.Find(updateEmpresa.Id);
            if (empresa is null)
            {
                return NotFound("Empresa no encontrada");
            }
            empresa.Naturaleza = updateEmpresa.Naturaleza;
            empresa.Identificacion = updateEmpresa.Identificacion;
            empresa.Ciudad = updateEmpresa.Ciudad;
            empresa.RazonSocial = updateEmpresa.RazonSocial;
            empresa.Direccion = updateEmpresa.Direccion;
            empresa.Telefono = updateEmpresa.Telefono;
            empresa.NombreContacto = updateEmpresa.NombreContacto;
            empresa.CargoContacto = updateEmpresa.CargoContacto;
            empresa.TelContacto = updateEmpresa.TelContacto;
            empresa.FactorGwp = updateEmpresa.FactorGwp;

            _context.SaveChanges();
            return Ok(empresa);
        }


    }
}
