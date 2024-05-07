namespace BackendEmisiones.Models
{


    public partial class Empresa
    {
        public int Id { get; set; }
        public string Naturaleza { get; set; }
        public string Identificacion { get; set; }
        public string Ciudad { get; set; }
        public string RazonSocial { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string NombreContacto { get; set; }
        public string CargoContacto { get; set; }
        public string TelContacto { get; set; }
        public int? FactorGwp { get; set; }
        public List<Planta>? Plantas { get; set; }
    }
}
