using System.Text.Json.Serialization;

namespace BackendEmisiones.Models
{
    public partial class Evidencia
    {
        public int Id { get; set; }
        public int UsuarioDeteccionId { get; set; }
        public DateTime? FechaDeteccion { get; set; }
        public bool FotoAntes { get; set; } = false;
        public bool Video { get; set; } = false;
        public DateTime? FechaReparacion { get; set; }
        public bool FotoDespues { get; set; } = false;
        public int UsuarioReparacionId { get; set; }
        public int EmisionFugitivaId { get; set; }
        [JsonIgnore]
        public EmisionFugitiva? EmisionFugitiva { get; set; }
    }
}
