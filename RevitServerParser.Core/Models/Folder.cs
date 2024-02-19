using System.Collections.Generic;
using System.Linq;

namespace RevitServerParser.Core
{
    public class Folder
    {
        public Folder? Parent { get; set; }
        public int FolderLevel { get; set; }
        public string? Name { get; set; }
        public bool IsNullContent { get; set; }
        public bool IsNullAnySubFolderOrThisFolder => IsAnySubFolderOrThisFolder_IsNull();
        public string? Host { get; set; }
        public int? Year { get; set; }
        public List<Model> Models { get; set; } = [];
        public List<Folder> Folders { get; set; } = [];
        public Folder()
        {
        }

        internal bool IsAnySubFolderOrThisFolder_IsNull()
        {
            if (IsNullContent) return true;

            return UtilsConstants.GetAllFolders(this).Any(x => x.IsNullContent);
        }

        public string GetPath()
        {
            var temp = new Folder() { Name = Name, Parent = Parent };
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
