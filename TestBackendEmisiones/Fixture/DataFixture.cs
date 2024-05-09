using BackendEmisiones.Models;
using Bogus;

namespace TestBackendEmisiones.Fixture
{
    internal class DataFixture
    {

        public static List<Empresa> GetEmpresas(int count, bool useNewSeed = false)
        {
            return GetEmpresaFaker(useNewSeed).Generate(count);
        }
        public static Empresa GetEmpresa(bool useNewSeed = false)
        {
            return GetEmpresas(1, useNewSeed)[0];
        }

        private static Faker<Empresa> GetEmpresaFaker(bool useNewSeed)
        {
            var seed = 0;
            if (useNewSeed)
            {
                seed = Random.Shared.Next(10, int.MaxValue);
            }
            return new Faker<Empresa>()
                .RuleFor(t => t.Id, o => 0)
                .RuleFor(t => t.Ciudad, (faker, t) => faker.Address.City())
                .RuleFor(t => t.Naturaleza, "Juridica")
                .RuleFor(t => t.RazonSocial, (faker, t) => faker.Company.CompanyName())
                .RuleFor(t => t.Direccion, (faker, t) => faker.Address.FullAddress())
                .RuleFor(t => t.Identificacion, "1234567")
                .RuleFor(t => t.CargoContacto, (faker, t) => faker.Random.Words())                
                .RuleFor(t => t.Telefono, (faker, t) => faker.Phone.PhoneNumber())
                .RuleFor(t => t.TelContacto, (faker, t) => faker.Phone.PhoneNumber())
                .RuleFor(t => t.NombreContacto, (faker, t) => faker.Name.FullName())
                .RuleFor(t => t.FactorGwp, 25)
                .UseSeed(seed);
        }

        public static List<Planta> GetPlantas(int count, int maxEmpresa, bool useNewSeed = false)
        {
            return GetPlantaFaker(useNewSeed, maxEmpresa).Generate(count);
        }
        public static Planta GetPlanta(int maxEmpresa, bool useNewSeed = false)
        {
            return GetPlantas(1, maxEmpresa, useNewSeed)[0];
        }

        private static Faker<Planta> GetPlantaFaker(bool useNewSeed, int maxEmpresa)
        {
            var seed = 0;
            if (useNewSeed)
            {
                seed = Random.Shared.Next(10, int.MaxValue);
            }
            return new Faker<Planta>()
                .RuleFor(t => t.Id, o => 0)
                .RuleFor(t => t.Ciudad, (faker, t) => faker.Address.City())
                .RuleFor(t => t.Nombre, (faker, t) => faker.Company.CompanyName())
                .RuleFor(t => t.EmpresaId, (faker, t) => faker.Random.Number(1, maxEmpresa))
                .UseSeed(seed);
        }

        public static List<Sistema> GetSistemas(int count, int maxPlanta, bool useNewSeed = false)
        {
            return GetSistemaFaker(useNewSeed, maxPlanta).Generate(count);
        }
        public static Sistema GetSistema(int maxPlanta, bool useNewSeed = false)
        {
            return GetSistemas(1, maxPlanta, useNewSeed)[0];
        }


        private static Faker<Sistema> GetSistemaFaker(bool useNewSeed, int maxPlanta)
        {
            var seed = 0;
            if (useNewSeed)
            {
                seed = Random.Shared.Next(10, int.MaxValue);
            }
            return new Faker<Sistema>()
                .RuleFor(t => t.Id, o => 0)
                .RuleFor(t => t.Nombre, (faker, t) => faker.Company.CompanyName())
                .RuleFor(t => t.Descripcion, (faker, t) => faker.Lorem.Sentence())
                .RuleFor(t => t.PlantaId, (faker, t) => faker.Random.Number(1, maxPlanta))
                .UseSeed(seed);
        }

        public static List<EmisionCombustion> GetEmisionesCombustion(int count, int maxSistema, bool useNewSeed = false)
        {
            return GetEmisionCombustionFaker(useNewSeed, maxSistema).Generate(count);
        }
        public static EmisionCombustion GetEmisionCombustion(int maxSistema, bool useNewSeed = false)
        {
            return GetEmisionesCombustion(1, maxSistema, useNewSeed)[0];
        }


        private static Faker<EmisionCombustion> GetEmisionCombustionFaker(bool useNewSeed, int maxSistema)
        {
            var seed = 0;
            if (useNewSeed)
            {
                seed = Random.Shared.Next(10, int.MaxValue);
            }
            return new Faker<EmisionCombustion>()
                .RuleFor(t => t.Id, o => 0)
                .RuleFor(t => t.Consecutivo, (faker, t) => "1234")
                .RuleFor(t => t.Tag, (faker, t) => faker.Random.Words())
                .RuleFor(t => t.Nombre, (faker, t) => faker.Random.Words())
                .RuleFor(t => t.Descripcion, (faker, t) => faker.Lorem.Sentence())
                .RuleFor(t => t.HorasOperacion, (faker, t) => faker.Random.Number(4000, 5000))
                .RuleFor(t => t.EficienciaCombustion, (faker, t) => faker.Random.Number(2, 2))
                .RuleFor(t => t.Observacion, (faker, t) => faker.Lorem.Sentence())
                .RuleFor(t => t.SistemaId, (faker, t) => faker.Random.Number(1, maxSistema))
                .RuleFor(t => t.TipoFuenteId, (faker, t) => faker.Random.Number(1, 4))
                .RuleFor(t => t.FactorEmisionId, (faker, t) => faker.Random.Number(1, 4))
                .UseSeed(seed);
        }

        public static List<EmisionFugitiva> GetEmisionesFugitivas(int count, int maxSistema, bool useNewSeed = false)
        {
            return GetEmisionFugitivaFaker(useNewSeed, maxSistema).Generate(count);
        }
        public static EmisionFugitiva GetEmisionFugitiva(int maxSistema, bool useNewSeed = false)
        {
            return GetEmisionesFugitivas(1, maxSistema, useNewSeed)[0];
        }

        private static Faker<EmisionFugitiva> GetEmisionFugitivaFaker(bool useNewSeed, int maxSistema)
        {
            var seed = 0;
            if (useNewSeed)
            {
                seed = Random.Shared.Next(10, int.MaxValue);
            }
            return new Faker<EmisionFugitiva>()
                .RuleFor(t => t.Id, o => 0)
                .RuleFor(t => t.Consecutivo, (faker, t) => "1234")
                .RuleFor(t => t.Tag, (faker, t) => faker.Random.Words())
                .RuleFor(t => t.Nombre, (faker, t) => faker.Random.Words())
                .RuleFor(t => t.Descripcion, (faker, t) => faker.Lorem.Sentence())
                .RuleFor(t => t.HorasOperacion, (faker, t) => faker.Random.Number(4000, 5000))
                .RuleFor(t => t.HorasOperacion, (faker, t) => faker.Random.Number(4000, 5000))
                .RuleFor(t => t.Observacion, (faker, t) => faker.Lorem.Sentence())
                .RuleFor(t => t.Tamano, (faker, t) => faker.Random.Number(20, 50) + "ft")
                .RuleFor(t => t.CaudalEmision, (faker, t) => faker.Random.Number(5, 25))
                .RuleFor(t => t.FechaDeteccion, (faker, t) => faker.Date.Past(5, DateTime.Now))
                .RuleFor(t => t.FechaReparacion, (faker, t) => faker.Date.Past(1, DateTime.Now))
                .RuleFor(t => t.FactorGwp, (faker, t) => faker.Random.Number(22, 28))
                .RuleFor(t => t.SistemaId, (faker, t) => faker.Random.Number(1, maxSistema))
                .RuleFor(t => t.TipoFuenteId, (faker, t) => faker.Random.Number(1, 4))
                .RuleFor(t => t.FactorEmisionId, (faker, t) => faker.Random.Number(1, 4))
                .UseSeed(seed);
        }


        public static List<Evidencia> GetEvidencias(int count, int maxEmision, bool useNewSeed = false)
        {
            return GetEvienciaFaker(useNewSeed, maxEmision).Generate(count);
        }
        public static Evidencia GetEvidencia(int maxEmision, bool useNewSeed = false)
        {
            return GetEvidencias(1, maxEmision, useNewSeed)[0];
        }

        private static Faker<Evidencia> GetEvienciaFaker(bool useNewSeed, int maxEmision)
        {
            var seed = 0;
            if (useNewSeed)
            {
                seed = Random.Shared.Next(10, int.MaxValue);
            }
            return new Faker<Evidencia>()
                .RuleFor(t => t.Id, o => 0)
                .RuleFor(t => t.UsuarioDeteccionId, o => 1)
                .RuleFor(t => t.UsuarioReparacionId, o => 1)
                .RuleFor(t => t.FechaDeteccion, (faker, t) => faker.Date.Past(1, DateTime.Now))
                .RuleFor(t => t.FechaReparacion, (faker, t) => faker.Date.Past(1, DateTime.Now))
                .RuleFor(t => t.FotoAntes, o => false)
                .RuleFor(t => t.FotoDespues, o => false)
                .RuleFor(t => t.Video, o => false)
                .RuleFor(t => t.EmisionFugitivaId, (faker, t) => faker.Random.Number(1, maxEmision))
                .UseSeed(seed);
        }
    }
}

