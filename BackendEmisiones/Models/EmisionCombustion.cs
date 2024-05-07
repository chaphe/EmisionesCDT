using System.Text.Json.Serialization;

namespace BackendEmisiones.Models
{
    public class EmisionCombustion
    {
        public int Id { get; set; }
        public string Consecutivo { get; set; }
        public string Tag { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int HorasOperacion { get; set; }
        public decimal EficienciaCombustion { get; set; }
        public string Observacion { get; set; }

        public int SistemaId { get; set; }
        [JsonIgnore]
        public Sistema? Sistema { get; set; }
        public int TipoFuenteId { get; set; }
        [JsonIgnore]
        public TipoFuente? TipoFuente { get; set; }
        public int FactorEmisionId { get; set; }
        [JsonIgnore]
        public FactorEmision? FactorEmision { get; set; }

        /*
         * Mirar si hay necesidad de historico de combustión
        public virtual ICollection<HistoricoCombustion> HistoricoCombustion { get; set; }
        */
    }
}
