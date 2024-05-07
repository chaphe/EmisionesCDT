using System.Text.Json.Serialization;

namespace BackendEmisiones.Models
{
    public class ReporteMensualGasDetalle
    {
        public int Id { get; set; }
        public int SistemaId { get; set; }
        public string Sistema { get; set; }
        public decimal Enision { get; set; }
        public decimal EmisionCO2 { get; set; }
        public int ReporteMensualGasId { get; set; }
        [JsonIgnore]
        public ReporteMensualGas ReporteMensual { get; set; }

    }
}
