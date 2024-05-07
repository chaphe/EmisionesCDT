using System.Text.Json.Serialization;

namespace BackendEmisiones.Models
{
    public class Sistema
    {


        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int PlantaId { get; set; }
        [JsonIgnore]
        public Planta? Planta { get; set; }
        public List<EmisionFugitiva>? EmisionesFugitivas { get; set; }
        public List<EmisionCombustion>? EmisionesCombustion { get; set; }

    }
}
