using Microsoft.Extensions.Logging;
using RevitServerParser.Parser;

namespace RevitServersService.Tests
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    public class ParseResultServiceTests
    {
        private HttpClient client;
        private ParseResultService service;

        public ParseResultServiceTests()
        {
            var f = new Microsoft.Extensions.Logging.Abstractions.NullLoggerFactory();
            client = new HttpClient();
            service = new ParseResultService(f.CreateLogger<ParseResultService>(), client);
        }

        [Test, Order(0)]
        public async Task Add()
        {
            var p = new ServerParser("srv4", 2022, client);

            var r = await p.ParseServer();

            Assert.DoesNotThrow(() => service.Add(new ParseResult(DateTime.Now, [r])));
        }

        [Test]
        public void GetAll()
        {
            var result = service.GetAll();

            Assert.That(result.Servers, Is.Not.Empty);
        }

        [Test]
        public void GetYear()
        {
            var result = service.Get(2022);

            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        public void GetYearEmpty()
        {
            var result = service.Get(2020);

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void GetHost()
        {
            var result = service.Get("srv4");

            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        public void GetHostEmpty()
        {
            var result = service.Get("srv41");

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void GetProjectByName()
        {
            var result = service.GetProjectByName("Крылатская 23");

            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        public void GetAllFoldersByName()
        {
            var result = service.GetAllFoldersByName("01_АР");

            Assert.That(result.Count(), Is.GreaterThan(2));
        }

        [Test]
        public void GetAllModelsByName()
        {
            var result = service.GetAllModelsByName("OLP_R22_JKV_AK_01_AR_K00.rvt");

            Assert.That(result.Count(), Is.EqualTo(1));
        }

        [Test]
        public void GetAllModelsByNameEmpty()
        {
            var result = service.GetAllModelsByName("testModelName");

            Assert.That(result.Count(), Is.EqualTo(0));
        }
    }
}
