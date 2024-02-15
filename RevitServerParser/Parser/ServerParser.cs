using RevitServerParser.Core;

namespace RevitServerParser.Parser
{
    public class ServerParser
    {
        private readonly HttpClient client;

        public string Host { get; }
        public int Year { get; }

        public ServerParser(string host, int year, HttpClient client)
        {
            Host = host;
            Year = year;
            this.client = client;
        }

        public async Task<RevitServer?> ParseServer(int maxFolderLevel = 1, CancellationToken cancellationToken = default)
        {
            var baseFolder = await client.GetFolderContent(Host, Year, cancellationToken: cancellationToken);

            if (baseFolder == null)
                return null;

            var folders = baseFolder.Folders ?? [];
            var models = baseFolder.Models ?? [];

            var result = new RevitServer(Host, Year);
            result.Models.AddRange(models.Select(x => new Model() { Name = x.Name}));
            result.Folders.AddRange(folders.Select(x => new Folder() { Name = x.Name, Host = Host, Year = Year }));

            var stack = new Stack<Folder>(result.Folders);

            while (stack.Count != 0)
            {
                var folder = stack.Pop();
                string path = folder.GetPath();

                var content = await client.GetFolderContent(Host, Year, path, cancellationToken).ConfigureAwait(false);
                if (content == null)
                {
                    folder.IsNullContent = true;
                    continue;
                }

                folders = content.Folders ?? [];
                models = content.Models ?? [];
                folder.Models.AddRange(models.Select(x => new Model() { Name = x.Name, Parent = folder }));

                if (folder.FolderLevel + 1 <= maxFolderLevel)
                {
                    folder.Folders.AddRange(folders.Select(x => new Folder(){Name =x.Name,Parent = folder, FolderLevel =folder.FolderLevel + 1}));
                }

                foreach (var f in folder.Folders)
                {
                    if (f.FolderLevel > maxFolderLevel) continue;
                    stack.Push(f);
                }

            }

            return result;
        }
    }
}