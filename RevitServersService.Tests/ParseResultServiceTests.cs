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
        public async Task GetAll()
        {
            var result = await service.GetAllAsync();

            Assert.That(result.Servers, Is.Not.Empty);
        }

        [Test]
        public async Task GetYear()
        {
            var result = await service.GetAsync(2022);

            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        public async Task GetYearEmpty()
        {
            var result = await service.GetAsync(2020);

            Assert.That(result, Is.Empty);
        }

        [Test]
        public async Task GetHost()
        {
            var result = await service.GetAsync("srv4");

            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        public async Task GetHostEmpty()
        {
            var result = await service.GetAsync("srv41");

            Assert.That(result, Is.Empty);
        }

        [Test]
        public async Task GetProjectByName()
        {
            var result = await service.GetProjectByNameAsync("Крылатская 23");

            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        public async Task GetAllFoldersByName()
        {
            var result = await service.GetAllFoldersByNameAsync("01_АР");

            Assert.That(result.Count(), Is.GreaterThan(2));
        }

        [Test]
        public async Task GetAllModelsByName()
        {
            var result = await service.GetAllModelsByNameAsync("OLP_R22_JKV_AK_01_AR_K00.rvt");

            Assert.That(result.Count(), Is.EqualTo(1));
        }

        [Test]
        public async Task GetAllModelsByNameEmpty()
        {
            var result = await service.GetAllModelsByNameAsync("testModelName");

            Assert.That(result.Count(), Is.EqualTo(0));
        }

        [Test]
        public async Task GetModelInfo()
        {
            var result = await service.GetModelInfoAsync("srv4", 2022, "Жуков проезд|01_АР|OLP_R22_JKV_AK_01_AR_K00.rvt", CancellationToken.None);

            var date = new DateTime(2000, 1, 1);
            Assert.That(result.DateModified, Is.GreaterThan(date));
        }
    }
}
