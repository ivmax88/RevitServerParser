using RevitServerParser;
using RevitServerParser.Models;
using RevitServerParser.RevitServerModels;

namespace RevitServersService
{
    public class ParseResultService
    {
        private List<ParseResult> results = [];
        private ParseResult? lastGoodParse;
        private readonly ILogger<ParseResultService> logger;
        private readonly HttpClient client;

        public ParseResultService(ILogger<ParseResultService> logger, HttpClient client)
        {
            this.logger = logger;
            this.client = client;
        }

        public void Add(ParseResult result)
        {
            results.Add(result);

            if (results.Count > 2)
                results.RemoveAt(0);

            var anyNull = false;
            foreach (var server in result.Servers)
            {
                if (server.Folders.Any(f => f.IsNullAnySubFolderOrThisFolder))
                {
                    break;
                }
            }
            if (!anyNull)
                lastGoodParse = result;
        }

        #region api calls


        public async Task<DateTime> GetLastDatetimeAsync()
        {
            if (results.Count == 0)
                return DateTime.MinValue;

            return await Task.Run(() => results.Last().Date);
        }


        public async Task<ParseResult> GetAllAsync()
        {
            if (results.Count == 0)
                return new ParseResult(DateTime.MinValue, []);

            return await Task.Run(() => results.Last());
        }

        public async Task<IEnumerable<RevitServer>> GetAsync(int year)
        {
            if (results.Count == 0)
                return Enumerable.Empty<RevitServer>();

            return await Task.Run(() => results.Last().Servers.Where(x => x.Year == year));
        }

        public async Task<IEnumerable<RevitServer>> GetAsync(string host)
        {
            if (results.Count == 0)
                return Enumerable.Empty<RevitServer>();

            return await Task.Run(() => results.Last().Servers.Where(x => x.Host == host));
        }

        public async Task<IEnumerable<Folder>> GetAllProjectsAsync()
        {
            if (results.Count == 0)
                return Enumerable.Empty<Folder>();

            return await Task.Run(() =>
            {
                return results.Last().Servers.SelectMany(x => x.Folders)
                .OrderByDescending(x => x.IsNullAnySubFolderOrThisFolder)
                .ThenBy(x => x.Name)
                .ThenBy(x => x.Year)
                .ThenBy(x => x.Host);
            });
        }

        public async Task<IEnumerable<Folder>> GetProjectByNameAsync(string projectName)
        {
            if (results.Count == 0)
                return Enumerable.Empty<Folder>();

            return await Task.Run(() =>
            {
                var result = results.Last().Servers.SelectMany(x => x.Folders).Where(x => x.Name?.ToUpper() == projectName.ToUpper());
                if (result.Any(x => x.IsNullAnySubFolderOrThisFolder))
                {
                    if (lastGoodParse == null) return Enumerable.Empty<Folder>();

                    result = lastGoodParse.Servers.SelectMany(x => x.Folders).Where(x => x.Name?.ToUpper() == projectName.ToUpper());
                }

                return result;
            });
        }

        public async Task<IEnumerable<Folder>> GetAllFoldersByNameAsync(string fodlerName)
        {
            if (results.Count == 0)
                return Enumerable.Empty<Folder>();

            return await Task.Run(() =>
            {
                return results.Last().Servers.SelectMany(s => s.Folders).SelectMany(UtilsConstants.GetAllFolders)
                .Where(f => string.Compare(f.Name, fodlerName, StringComparison.CurrentCultureIgnoreCase) == 0);
            });
        }

        public async Task<IEnumerable<Model>> GetAllModelsAsync()
        {
            if (results.Count == 0)
                return Enumerable.Empty<Model>();

            return await Task.Run(() =>
            {
                var result = results.Last();
                if (result.Servers.SelectMany(x => x.Folders).Any(x => x.IsNullAnySubFolderOrThisFolder))
                {
                    if (lastGoodParse != null)
                        result = lastGoodParse;
                }
                return result.Servers.SelectMany(s => s.Folders).SelectMany(UtilsConstants.GetAllModels);
            });
        }

        public async Task<IEnumerable<Model>> GetAllModelsByNameAsync(string modelName)
        {
            var models = await GetAllModelsAsync();
            if (!models.Any())
                return Enumerable.Empty<Model>();

            return models.Where(f => string.Compare(f.Name, modelName, StringComparison.CurrentCultureIgnoreCase) == 0);
        }

        public async Task<History> GetModelHistoryAsync(string host, int year, string path, CancellationToken token)
        {
            if (!path.ToUpper().EndsWith(".RVT"))
                throw new ArgumentException("Path must be end with .rvt", nameof(path));

            if (host == null || path == null || year < 2015)
                return new History();

            return await client.GetHistory(host, year, path, token) ?? new History();
        }

        public async Task<Modelnfo> GetModelInfoAsync(string host, int year, string path, CancellationToken token)
        {
            if (!path.ToUpper().EndsWith(".RVT"))
                throw new ArgumentException("Path must be end with .rvt", nameof(path));

            if (host == null || path == null || year < 2015)
                return new Modelnfo();

            return await client.GetModelInfo(host, year, path, token) ?? new Modelnfo();
        }

        public async Task<IEnumerable<string>> GetPathByModelName(string modelName)
        {
            var models = await GetAllModelsByNameAsync(modelName);
            if (models.Count() == 0) return Enumerable.Empty<string>();

            models = models.Where(x => string.Compare(x.Name, modelName, StringComparison.OrdinalIgnoreCase) == 0);

            if (models.Count() == 0) return Enumerable.Empty<string>();

            return models.Select(x => x.ToString());
        }

        public async Task<IEnumerable<string>> GetModelsPathsAsync(IEnumerable<string> modelNames)
        {
            var models = await GetAllModelsAsync();
            var result = new List<string>();

            foreach (var m in models)
            {
                var name = modelNames.FirstOrDefault(x => string.Compare(x, m.Name, StringComparison.CurrentCultureIgnoreCase) == 0);
                if (name == null) continue;
                result.Add(m.ToString());
            }

            return result;
        }


        #endregion
    }
}
