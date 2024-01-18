using System.Text.Json.Serialization;

namespace RevitServerParser.Models
{
    public class Folder
    {
        [JsonIgnore]
        internal Folder? Parent { get; }
        public int FolderLevel { get; }
        public string? Name { get; }
        public bool IsNullContent { get; set; }
        public bool IsNullAnySubFolderOrThisFolder => IsAnySubFolderOrThisFolder_IsNull();
        public string? Host { get; }
        public int? Year { get; }
        public List<Model> Models { get; } = [];
        public List<Folder> Folders { get; } = [];

        internal Folder(string? name, Folder? parent, int level = 0)
        {
            Name = name;
            Parent = parent;
            FolderLevel = level;
        }

        internal Folder(string? name, string host, int year)
        {
            Name = name;
            Host = host;
            Year = year;
        }

        [JsonConstructor]
        public Folder(string? name, int folderLevel,
             List<Folder> folders, List<Model> models, 
             string? host, int? year)
        {
            Name = name;
            FolderLevel = folderLevel;
            Folders = folders;
            Models = models;
            Host = host;
            Year = year;
        }

        internal bool IsAnySubFolderOrThisFolder_IsNull()
        {
            if (IsNullContent) return true;

            return UtilsConstants.GetAllFolders(this).Any(x => x.IsNullContent);
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
