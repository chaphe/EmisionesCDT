namespace BackendEmisiones.Dtos
{
    public class AddEmisionFugitivaDto
    {

        public int FactorEmisionId { get; set; }
        public string Consecutivo { get; set; }
        public string Tag { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int HorasOperacion { get; set; }
        public string Tamano { get; set; }
        public decimal? Presion { get; set; }
        public decimal? Temperatura { get; set; }
        public bool Fuga { get; set; }
        public decimal CaudalEmision { get; set; }
        public int? FactorGwp { get; set; }
        public string Observacion { get; set; }
        public DateTime? FechaDeteccion { get; set; }
        public DateTime? FechaReparacion { get; set; }
        public int SistemaId { get; set; }
        public int TipoFuenteId { get; set; }

    }
}
