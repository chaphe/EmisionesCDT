namespace BackendEmisiones.Models
{
    public class ReporteMensualGas
    {
        public int Id { get; set; }
        public int EmpresaId { get; set; }
        public string Empresa { get; set; }
        public int PlantaId { get; set; }
        public string Planta { get; set; }
        public int GasId { get; set; }
        public string Gas { get; set; }
        public int Anho { get; set; }
        public int Mes { get; set; }
        public List<ReporteMensualGasDetalle> Items { get; set; }

    }
}
