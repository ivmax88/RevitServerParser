using RevitServerParser.Models;

namespace RevitServersService
{
    public class ParseResultService
    {
        private List<ParseResult> results = [];

        public ParseResultService()
        {
            
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

            return results.Last().Servers.SelectMany(x=>x.Folders).Where(x=>x.Name == projectName);
        }

        public IEnumerable<Folder> GetAllFoldersByName(string fodlerName)
        {
            if (results.Count == 0)
                return Enumerable.Empty<Folder>();

            return results.Last().Servers.SelectMany(s=>s.Folders).SelectMany(GetAllFolders)
                .Where(f=>f.Name == fodlerName);
        }

        public IEnumerable<Model> GetAllModelsByName(string modelName)
        {
            if (results.Count == 0)
                return Enumerable.Empty<Model>();

            return results.Last().Servers.SelectMany(s => s.Folders).SelectMany(GetAllModels)
                .Where(f => f.Name == modelName);
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
