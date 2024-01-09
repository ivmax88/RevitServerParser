using System.Text.Json.Serialization;

namespace RevitServerParser.Models
{
    public class Folder
    {
        [JsonIgnore]
        internal Folder? Parent { get; }
        public int FolderLevel { get; }
        public string? Name { get; }
        public List<Model> Models { get; } = [];
        public List<Folder> Folders { get; } = [];

        internal Folder(string? name, Folder? parent = null, int level = 0)
        {
            Name = name;
            Parent = parent;
            FolderLevel = level;
        }

        [JsonConstructor]
        public Folder(string? name, int folderLevel,
             List<Folder> folders, List<Model> models)
        {
            Name = name;
            FolderLevel = folderLevel;
            Folders = folders;
            Models = models;
        }

        internal string GetPath()
        {
            var temp = new Folder(Name, Parent);
            var folderPaths = new List<string>() { temp.Name! };
            while (temp.Parent != null)
            {
                folderPaths.Add(temp.Parent!.Name!);
                temp = temp.Parent;
            }
            folderPaths.Reverse();
            var path = string.Join("|", folderPaths);
            return path;
        }

        public override string ToString()
        {
            return GetPath();
        }
    }
}
