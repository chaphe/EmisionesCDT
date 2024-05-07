using AutoMapper;
using BackendEmisiones.Dtos;
using BackendEmisiones.Models;

namespace BackendEmisiones
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<AddEmpresaDto, Empresa>();
            CreateMap<AddPlantaDto, Planta>();
            CreateMap<AddSistemaDto, Sistema>();
            CreateMap<AddEmisionFugitivaDto, EmisionFugitiva>();
            CreateMap<AddEmisionCombustionDto, EmisionCombustion>();
            CreateMap<AddEvidenciaDto, Evidencia>();
        }
    }
}
