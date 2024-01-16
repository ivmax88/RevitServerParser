using System.Reflection.Metadata;
using RevitServerParser;
using RevitServerParser.Models;
using RevitServerParser.RevitServerModels;

namespace RevitServersService
{
    public class ParseResultService
    {
        private List<ParseResult> results = [];
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
        }

        public ParseResult GetAll()
        {
            if (results.Count == 0)
                return new ParseResult(DateTime.MinValue, []);
                
            return results.Last();
        }

        public IEnumerable<RevitServer> Get(int year)
        {
            if (results.Count == 0)
                return Enumerable.Empty<RevitServer>();

            return results.Last().Servers.Where(x => x.Year == year);
        }

        public IEnumerable<RevitServer> Get(string host)
        {
            if (results.Count == 0)
                return Enumerable.Empty<RevitServer>();

            return results.Last().Servers.Where(x => x.Host == host);
        }


        public IEnumerable<Folder> GetProjectByName(string projectName)
        {
            if (results.Count == 0)
                return Enumerable.Empty<Folder>();

            return results.Last().Servers.SelectMany(x=>x.Folders).Where(x=>x.Name?.ToUpper() == projectName.ToUpper());
        }

        public IEnumerable<Folder> GetAllFoldersByName(string fodlerName)
        {
            if (results.Count == 0)
                return Enumerable.Empty<Folder>();

            return results.Last().Servers.SelectMany(s=>s.Folders).SelectMany(GetAllFolders)
                .Where(f=>f.Name?.ToUpper() == fodlerName.ToUpper());
        }

        public IEnumerable<Model> GetAllModelsByName(string modelName)
        {
            if (results.Count == 0)
                return Enumerable.Empty<Model>();

            return results.Last().Servers.SelectMany(s => s.Folders).SelectMany(GetAllModels)
                .Where(f => f.Name?.ToUpper() == modelName.ToUpper());
        }

        public async Task<History> GetModelHistory(string host, int year, string path, CancellationToken token)
        {
            if(!path.ToUpper().EndsWith(".RVT"))
                throw new ArgumentException("Path must be end with .rvt", nameof(path));

            if (host  == null || path == null || year < 2015)
                return new History();

            return await client.GetHistory(host, year, path, token) ?? new History();
        }

        public async Task<Modelnfo> GetModelInfo(string host, int year, string path, CancellationToken token)
        {
            if (!path.ToUpper().EndsWith(".RVT"))
                throw new ArgumentException("Path must be end with .rvt", nameof(path));

            if (host == null || path == null || year < 2015)
                return new Modelnfo();

            return await client.GetModelInfo(host, year, path, token) ?? new Modelnfo();
        }

        private IEnumerable<Folder> GetAllFolders(Folder folder)
        {
            var q = new Queue<Folder>();
            q.Enqueue(folder);

            var r = new List<Folder>();

            while(q.Count > 0)
            {
                var f = q.Dequeue();

                r.Add(f);

                if(f.Folders?.Count > 0)
                    foreach(var ff in f.Folders)
                        q.Enqueue(ff);
            }

            return r;
        }

        private IEnumerable<Model> GetAllModels(Folder folder)
        {
            var q = new Queue<Folder>();
            q.Enqueue(folder);

            var r = new List<Model>();

            while (q.Count > 0)
            {
                var f = q.Dequeue();

                r.AddRange(f.Models.Where(x=>x is not null));

                if (f.Folders?.Count > 0)
                    foreach (var ff in f.Folders)
                        q.Enqueue(ff);
            }

            return r;
        }
    }
}
