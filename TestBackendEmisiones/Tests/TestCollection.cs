using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestBackendEmisiones.Fixture;

namespace TestBackendEmisiones.Tests
{
    [CollectionDefinition("Backend EmisionCDT Collection")]
    public class TestCollection : IClassFixture<WebApplicationFactoryFixture>
    {
    }
}
