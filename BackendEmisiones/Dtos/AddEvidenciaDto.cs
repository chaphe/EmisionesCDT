namespace BackendEmisiones.Dtos
{
    public partial class AddEvidenciaDto
    {
        public string UsuarioDeteccionId { get; set; }
        public DateTime FechaDeteccion { get; set; }
        public bool FotoAntes { get; set; }
        public bool Video { get; set; }
        public DateTime FechaReparacion { get; set; }
        public bool FotoDespues { get; set; }
        public string IdUsuarioReparacion { get; set; }
        public int EmisionFugitivaId { get; set; }
    }
}
