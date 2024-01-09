namespace RevitServerParser.Models
{
    public class Folder
    {
        public Folder? Parent { get; }
        public string? Name { get; }
        public List<Model> Models { get; } = [];
        public List<Folder> Folders { get; } = [];
        public Folder(string? name, Folder? parent = null)
        {
            Name = name;
            Parent = parent;
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
