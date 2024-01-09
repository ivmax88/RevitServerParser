using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RevitServerParser.Parser;

namespace RevitServerParser.Tests
{
    internal class ParserTests
    {
        private static IEnumerable<object[]> Servers()
        {
            yield return new object[] { "srv4", 2022 };
        }

        [TestCaseSource(nameof(Servers))]
        public async Task TesrServerParser(string host, int year)
        {
            var client = Client.GetClient();
            var parser = new ServerParser(host, year, client);

            var server = await parser.ParseServer();

            Assert.That(server, Is.Not.Null);
        }
    }
}
