namespace BackendEmisiones.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        public string TipoDocumento { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string Cargo { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }
        public string Identificacion { get; set; }
        public string NombreUsuario { get; set; }
        public string Contraseña { get; set; }
    }
}
