namespace BackendEmisiones.Models
{
    public class TipoFuente
    {
        public int Id { get; set; }
        public int? IdClasificacion { get; set; }
        public string Nombre { get; set; }
        public int TipoEmision { get; set; }
        public string? Observacion { get; set; }
        public List<EmisionFugitiva> EmisionesFugitivas { get; set; }

    }
}
