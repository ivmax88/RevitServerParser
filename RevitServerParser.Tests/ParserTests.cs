using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Utilities;
using RevitServerParser.Models;
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


            var server = await parser.ParseServer(2);


            Assert.That(server, Is.Not.Null);

            var json = JsonSerializer.Serialize(server);

            server = JsonSerializer.Deserialize<RevitServer>(json);

            Assert.That(server, Is.Not.Null);
            Assert.That(server.Folders, Is.Not.Empty);
        }

    }
}
