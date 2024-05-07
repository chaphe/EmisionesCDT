namespace BackendEmisiones.Models
{
    public class ComposicionGas
    {
        public int Id { get; set; }
        public string NombreGas { get; set; }
        public decimal? Oxigeno { get; set; }
        public decimal? Nitrogeno { get; set; }
        public decimal Metano { get; set; }
        public decimal DioxidoCarbono { get; set; }
        public decimal? Etano { get; set; }
        public decimal? Propano { get; set; }
        public decimal? Isobutano { get; set; }
        public decimal? Nbutano { get; set; }
        public decimal? Isopentano { get; set; }
        public decimal? Npentano { get; set; }
        public decimal? NhexanoIsomeros { get; set; }
        public decimal? NheptanoIsomeros { get; set; }
        public decimal? NoctanoIsomeros { get; set; }
        public decimal? NnonanoIsomeros { get; set; }
        public decimal? NdecanoIsomeros { get; set; }
        public decimal? NundecanoIsomeros { get; set; }
        public decimal? NdodecanoIsomeros { get; set; }

    }
}
