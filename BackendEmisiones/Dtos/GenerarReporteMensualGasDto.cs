namespace BackendEmisiones.Dtos
{
    public class GenerarReporteMensualGasDto
    {
        public int PlantaID { get; set; }
        public int Mes { get; set; }
        public int Anho { get; set; }
        public int GasId { get; set; }
    }
}
