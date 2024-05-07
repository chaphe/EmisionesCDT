using BackendEmisiones.Models;
using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestEmisionesCDT.Fixture
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

    }
}

