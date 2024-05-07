using System.Text.Json.Serialization;

namespace BackendEmisiones.Models
{
    public class Planta
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Ciudad { get; set; }
        public int EmpresaId { get; set; }
        [JsonIgnore]
        public Empresa? Empresa { get; set; }
        public List<Sistema>? Sistemas { get; set; }

    }
}
