using System.Data;
using RevitServerParser.Core;
using RevitServerParser.Parser;
using System.Drawing;

namespace RevitServerParser.Tests
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    public class TestParserExtensions 
    {
        [Test]
        [TestCaseSource(nameof(Servers))]
        public async Task Test_GetFolderContent(string host, int year)
        {
            var client = Client.GetClient();

            var folder = await client.GetFolderContent(host, year);

            Assert.That(folder, Is.Not.Null);
            Assert.That(folder.Folders, Is.Not.Null);
            Assert.That(folder.Folders, Is.Not.Empty);
        }

        [Test]
        [TestCaseSource(nameof(Servers))]
        public async Task Test_GetServerProperties(string host, int year)
        {
            var client = Client.GetClient();

            var prop = await client.GetServerProperties(host, year);

            Assert.That(prop, Is.Not.Null);
            Assert.That(prop.Servers, Is.Not.Null);
            Assert.That(prop.Servers, Is.Not.Empty);
        }

        [Test]
        [TestCaseSource(nameof(Directory))]
        public async Task Test_GetDirectoryInfo(string host, int year, string projectName)
        {
            var client = Client.GetClient();

            var dir = await client.GetDirectoryInfo(host, year, projectName);

            Assert.That(dir, Is.Not.Null);
        }

        [Test]
        [TestCaseSource(nameof(Models))]
        public async Task Test_GetHistory(string host, int year, string projectName, string secttion, string model)
        {
            var client = Client.GetClient();

            var hist = await client.GetHistory(host, year, $"{projectName}|{secttion}|{model}");

            Assert.That(hist, Is.Not.Null);
            Assert.That(hist.Items, Is.Not.Null);
            Assert.That(hist.Items, Is.Not.Empty);
        }

        [Test]
        [TestCaseSource(nameof(Models))]
        public async Task Test_GetModelInfo(string host, int year, string projectName, string secttion, string model)
        {
            var client = Client.GetClient();

            var modelnfo = await client.GetModelInfo(host, year, $"{projectName}|{secttion}|{model}");

            Assert.That(modelnfo, Is.Not.Null);
        }


        [Test]
        [TestCaseSource(nameof(Models))]
        public async Task Test_GetThumbnail(string host, int year, string projectName, string secttion, string model)
        {
            var client = Client.GetClient();

            var ar = await client.GetThumbnail(host, year, 128, 128, $"{projectName}|{secttion}|{model}");

            Assert.That(ar, Is.Not.Null);
        }

        private static IEnumerable<object[]> Servers()
        {
            yield return new object[] { "srv4", 2022 };
            yield return new object[] { "srv2", 2022 };
        }

        private static IEnumerable<object[]> Directory()
        {
            yield return new object[] { "srv4", 2022 , "Фили 5.2"};
        }
        private static IEnumerable<object[]> Models()
        {
            yield return new object[] { "srv2", 2020, "Котляковская", "00_КООРД", "OLP_R20_KTLK_BASE.rvt" };
            yield return new object[] { "srv1", 2019, "Тест", "01_АР", "ivanov_newtask_test_AR.rvt" };
        }

    }
}
