using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestEmisionesCDT.Fixture;

namespace TestEmisionesCDT.Tests
{
    [CollectionDefinition("EmisionCDT Collection")]
    public class TestCollection : IClassFixture<WebApplicationFactoryFixture>
    {
    }
}
