namespace BackendEmisiones.Models
{
    public partial class FactorEmision
    {

        public int Id { get; set; }
        public string NombreGas { get; set; }
        public decimal ValorCo2combustion { get; set; }
        public decimal ValorCh4fugitivas { get; set; }
        public decimal ValorCo2fugitivas { get; set; }

        public int? ComposicionGasId { get; set; }
        public ComposicionGas? ComposicionGas { get; set; }
        public List<EmisionCombustion>? EmisionesCombustion { get; set; }
        public List<EmisionFugitiva>? EmisionesFugitivas { get; set; }
    }
}
