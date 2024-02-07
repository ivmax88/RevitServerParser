using System.Text.Json.Serialization;

namespace RevitServerParser.RevitServerModels
{
    public class DirectoryInfo
    {
        public string? Path { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public bool Exists { get; set; }
        public int FolderCount { get; set; }
        public bool IsFolder { get; set; }
        public object? LastModifiedBy { get; set; }
        public object? LockContext { get; set; }
        public int LockState { get; set; }
        public int ModelCount { get; set; }
        public List<ModelLocksInProgress>? ModelLocksInProgress { get; set; }
        public long ModelSize { get; set; }
        public long Size { get; set; }
    }
}
