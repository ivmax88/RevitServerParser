using System.Data;
using RevitServerParser.RevitServerModels;
using RevitServerParser.Models;
using RevitServerParser.Parser;

namespace RevitServerParser.Tests
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    public class Tests 
    {
        [Test]
        [TestCaseSource(nameof(Servers))]
        public async Task TestRevitServerParse(string host, int year)
        {
            var client = Client.GetClient();

            var folder = await client.GetFolderContent(host, year);

            Assert.That(folder, Is.Not.Null);
            Assert.That(folder.Folders, Is.Not.Null);
            Assert.That(folder.Folders, Is.Not.Empty);
        }

        private static IEnumerable<object[]> Servers()
        {
            yield return new object[] { "srv4", 2022 };
            yield return new object[] { "srv2", 2022 };
        }

       
    }
}
