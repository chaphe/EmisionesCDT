using System.Text.Json.Serialization;

namespace BackendEmisiones.Models
{
    public class ReporteMensualDetalle
    {
        public int Id { get; set; }
        public int FactorEmisionId { get; set; }
        public string NombreGas { get; set; }
        public int SistemaId { get; set; }
        public string Sistema { get; set; }
        public decimal Enision { get; set; }
        public decimal EmisionCO2 { get; set; }
        public int ReporteMensualId { get; set; }
        [JsonIgnore]
        public ReporteMensual ReporteMensual { get; set; }

    }
}
