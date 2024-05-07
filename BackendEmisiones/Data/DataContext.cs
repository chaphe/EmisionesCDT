using BackendEmisiones.Models;
using Microsoft.EntityFrameworkCore;

namespace BackendEmisiones.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<Planta> Plantas { get; set; }
        public DbSet<Sistema> Sistemas { get; set; }
        public DbSet<EmisionFugitiva> EmisionesFugitivas { get; set; }
        public DbSet<EmisionCombustion> EmisionesCombustion { get; set; }
        public DbSet<TipoFuente> TipoFuente { get; set; }
        public DbSet<FactorEmision> FactoresEmision { get; set; }
        public DbSet<ComposicionGas> ComposicionesGas { get; set; }
        public DbSet<Evidencia> Evidencias { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }


        /*
         * Sección de Reportes
         */
        public DbSet<ReporteMensual> ReporteMensual { get; set; }
        public DbSet<ReporteMensualDetalle> ReporteMensualDetalle { get; set; }
        public DbSet<ReporteMensualGas> ReporteMensualGas { get; set; }
        public DbSet<ReporteMensualGasDetalle> ReporteMensualGasDetalle { get; set; }

    }
}
