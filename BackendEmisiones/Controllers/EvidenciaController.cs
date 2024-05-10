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
    public class EvidenciaController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly DataContext _context;
        private readonly IMapper _mapper;


        public EvidenciaController(IWebHostEnvironment webHostEnvironment, DataContext context, IMapper mapper)
        {
            _webHostEnvironment = webHostEnvironment;
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("/Imagen/Deteccion/{id}")]
        public ActionResult GetImagenDeteccion(int id)
        {
            string contentRootPath = _webHostEnvironment.ContentRootPath;
            string rutaImagen = contentRootPath + "\\Evidencias\\Antes" + id + ".jpg";
            return PhysicalFile(rutaImagen, "image/jpeg", "Antes" + id + ".jpg");
        }

        [HttpGet("/Imagen/Reparacion/{id}")]
        public ActionResult GetImagenReparacion(int id)
        {
            string contentRootPath = _webHostEnvironment.ContentRootPath;
            string rutaImagen = contentRootPath + "\\Evidencias\\Despues" + id + ".jpg";
            return PhysicalFile(rutaImagen, "image/jpeg", "evDespues" + id + ".jpg");
        }

        [HttpGet("GetSet/{EmisionId}")]
        public ActionResult<List<Evidencia>> GetSetPlantas(int EmisionId)
        {
            var emision = _context.EmisionesFugitivas.Find(EmisionId);
            if (emision is null)
                return NotFound("No existe la emision consultada");
            
            var evidencias = _context.Evidencias
                .Where(e => e.EmisionFugitivaId == EmisionId).ToList();
            return Ok(evidencias);
        }

        [HttpPost]
        public ActionResult<Evidencia> AddEvidencia(AddEvidenciaDto newEvidencia)
        {
            //_context.Empresas.Include(e => e.Plantas);
            var emision = _context.EmisionesFugitivas.Include(e => e.Evidencias)
                .FirstOrDefault(e => e.Id == newEvidencia.EmisionFugitivaId);
            if (emision is null)
            {
                return NotFound("No existe la emision fugitiva asociada");
            }
            var evidencia = _mapper.Map<Evidencia>(newEvidencia);
            evidencia.EmisionFugitiva = emision;
            emision.Evidencias.Add(evidencia);
            _context.SaveChanges();
            return Ok(evidencia);
        }

        [HttpGet("{id}")]
        public ActionResult<Evidencia> GetEvidenciaById(int id)
        {
            var evidencia = _context.Evidencias.Find(id);
            if (evidencia is null)
            {
                return NotFound("Evidencia no encontrada");
            }
            return Ok(evidencia);
        }

        [HttpPut]
        public ActionResult<Evidencia> UpdateEvidencia(Evidencia updatedEvidencia)
        {
            var evidencia = _context.Evidencias.Find(updatedEvidencia.Id);
            if (evidencia is null)
                return NotFound("Evidencia no encontrada");

            var emision = _context.EmisionesFugitivas.Find(updatedEvidencia.EmisionFugitivaId);
            if (emision is null)
                return NotFound("Emision no encontrada");

            evidencia.FechaDeteccion = updatedEvidencia.FechaDeteccion;
            evidencia.FechaReparacion = updatedEvidencia.FechaReparacion;
            evidencia.UsuarioDeteccionId = updatedEvidencia.UsuarioDeteccionId;
            evidencia.UsuarioReparacionId = updatedEvidencia.UsuarioReparacionId;
            evidencia.EmisionFugitivaId = updatedEvidencia.EmisionFugitivaId;
            evidencia.FotoAntes = updatedEvidencia.FotoAntes;
            evidencia.FotoDespues = updatedEvidencia.FotoDespues;
            evidencia.Video = updatedEvidencia.Video;
            evidencia.EmisionFugitiva = emision;

            _context.SaveChanges();
            return Ok(evidencia);
        }




        // POST: Imagen/Subir
        [HttpPost("SubirImagen/Deteccion")]
        public async Task<ActionResult> SubirImagenDeteccion(IFormFile archivo, int EvidenciaId)
        {
            return await SubirArchivo(archivo, EvidenciaId, true);
        }

        [HttpPost("SubirImagen/Reparacion")]
        public async Task<ActionResult> SubirImagenReparacion(IFormFile archivo, int EvidenciaId)
        {
            return await SubirArchivo(archivo, EvidenciaId, false);
        }

        private async Task<ActionResult> SubirArchivo(IFormFile archivo, int EvidenciaId, bool antes)
        {
            var evidencia = _context.Evidencias.Find(EvidenciaId);
            if (evidencia is  null)
            {
                return NotFound("Evidencia asociada no existe");
            }
            string mensaje = string.Empty;

            if (archivo != null && archivo.Length > 0)
            {
                // Verifica que el archivo sea una imagen JPEG
                var extension = Path.GetExtension(archivo.FileName).ToLower();
                if (extension == ".jpeg" || extension == ".jpg")
                {
                    var nuevoNombreArchivo = string.Empty;
                    if (antes)
                    {
                        nuevoNombreArchivo = "Antes" + EvidenciaId + ".jpg";
                        evidencia.FotoAntes = true;
                    }
                    else
                    {
                        nuevoNombreArchivo = "Despues" + EvidenciaId + ".jpg";
                        evidencia.FotoDespues = true;
                    }

                    var rutaParaGuardar = Path.Combine(Directory.GetCurrentDirectory(), "Evidencias", nuevoNombreArchivo);

                    using (var stream = new FileStream(rutaParaGuardar, FileMode.Create))
                    {
                        await archivo.CopyToAsync(stream);
                    }

                    _context.SaveChanges();

                    return Ok("El archivo se subio correctamente");
                }
                else
                {
                    return NotFound("El archivo no es una imagen JPEG.");
                }
            }
            else
            {
                return BadRequest("No se seleccionó ningún archivo.");
            }
        }


    }
}
