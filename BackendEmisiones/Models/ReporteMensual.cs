namespace BackendEmisiones.Models
{
    public class ReporteMensual
    {
        public int Id { get; set; }
        public int EmpresaId { get; set; }
        public string Empresa { get; set; }
        public int PlantaId { get; set; }
        public string Planta { get; set; }
        public int Anho { get; set; }
        public int Mes { get; set; }
        public List<ReporteMensualDetalle> Items { get; set; }

    }
}
